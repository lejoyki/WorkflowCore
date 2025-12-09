using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCore.Services.BackgroundTasks
{
    internal class WorkflowConsumer : QueueConsumer, IBackgroundTask
    {
        private readonly IPersistenceProvider _persistenceStore;
        private readonly IWorkflowExecutor _executor;

        protected override int MaxConcurrentItems => Options.MaxConcurrentWorkflows;
        protected override QueueType Queue => QueueType.Workflow;

        public WorkflowConsumer(IPersistenceProvider persistenceProvider,
        IQueueProvider queueProvider,
        ILoggerFactory loggerFactory,
        IWorkflowExecutor executor,
        WorkflowOptions options)
            : base(queueProvider, loggerFactory, options)
        {
            _persistenceStore = persistenceProvider;
            _executor = executor;
        }

        protected override async Task ProcessItem(string itemId, CancellationToken cancellationToken)
        {
            WorkflowInstance? workflow = null;
            WorkflowExecutorResult? result = null;

            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                workflow = await _persistenceStore.GetWorkflowInstance(itemId, cancellationToken);

                if (workflow?.Status == WorkflowStatus.Runnable)
                {
                    try
                    {
                        result = await _executor.Execute(workflow, cancellationToken);
                    }
                    finally
                    {
                        await _persistenceStore.PersistWorkflow(workflow, result?.Subscriptions ?? [], cancellationToken);
                    }
                }
            }
            finally
            {
                if ((workflow != null) && (result != null))
                {
                    // 检查订阅的事件
                    foreach (var sub in result.Subscriptions)
                    {
                        await TryProcessSubscription(sub, _persistenceStore, cancellationToken);
                    }

                    if ((workflow.Status == WorkflowStatus.Runnable) && workflow.NextExecution.HasValue)
                    {
                        new Task(() => FutureQueue(workflow, cancellationToken)).Start();
                    }
                }
            }
        }

        private async Task TryProcessSubscription(EventSubscription subscription, IPersistenceProvider persistenceStore, CancellationToken cancellationToken)
        {
            var events = await persistenceStore.GetEvents(subscription.EventName, subscription.EventKey, subscription.SubscribeAsOf, cancellationToken);

            foreach (var evt in events)
            {
                await persistenceStore.MarkEventUnprocessed(evt, cancellationToken);
                await QueueProvider.QueueWork(evt, QueueType.Event);
            }
        }

        private async void FutureQueue(WorkflowInstance workflow, CancellationToken cancellationToken)
        {
            try
            {
                if (!workflow.NextExecution.HasValue)
                {
                    return;
                }

                var target = workflow.NextExecution.Value - DateTime.Now.Ticks;
                if (target > 0)
                {
                    await Task.Delay(TimeSpan.FromTicks(target), cancellationToken);
                }

                await QueueProvider.QueueWork(workflow.Id, QueueType.Workflow);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }
        }
    }
}