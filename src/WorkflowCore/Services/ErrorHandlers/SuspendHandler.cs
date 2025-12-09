using System;
using System.Collections.Generic;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Models.LifeCycleEvents;

namespace WorkflowCore.Services.ErrorHandlers
{
    public class SuspendHandler : IWorkflowErrorHandler
    {
        private readonly ILifeCycleEventHub _lifeCycleEventHub;
        public WorkflowErrorHandling Type => WorkflowErrorHandling.Suspend;

        public SuspendHandler(ILifeCycleEventHub eventPublisher)
        {
            _lifeCycleEventHub = eventPublisher;
        }

        public void Handle(WorkflowInstance workflow, WorkflowDefinition def, ExecutionPointer pointer, WorkflowStep step, Exception exception, Queue<ExecutionPointer> bubbleUpQueue)
        {
            workflow.Status = WorkflowStatus.Suspended;
            _lifeCycleEventHub.PublishNotification(new WorkflowLifeCycleEvent
            {
                EventTime = DateTime.Now,
                WorkflowInstanceId = workflow.Id,
                WorkflowName = workflow.WorkflowName,
                Version = workflow.Version,
                WorkflowStatus = workflow.Status,
                CompleteTime = workflow.CompleteTime,
                CreatTime = workflow.CreateTime
            });
        }
    }
}
