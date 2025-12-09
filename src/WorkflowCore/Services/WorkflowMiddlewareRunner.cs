using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCore.Services
{
    /// <inheritdoc cref="IWorkflowMiddlewareRunner" />
    public class WorkflowMiddlewareRunner(
        IEnumerable<IWorkflowMiddleware> middleware,
        IServiceProvider serviceProvider) : IWorkflowMiddlewareRunner
    {
        private static readonly WorkflowDelegate NoopWorkflowDelegate = () => Task.CompletedTask;
        private readonly IEnumerable<IWorkflowMiddleware> _middleware = middleware;

        /// <inheritdoc cref="IWorkflowMiddlewareRunner.RunPreMiddleware"/>
        public async Task RunPreMiddleware(WorkflowInstance workflow, WorkflowDefinition def)
        {
            var preMiddleware = _middleware
                .Where(m => m.Phase == WorkflowMiddlewarePhase.PreWorkflow);

            await RunWorkflowMiddleware(workflow, preMiddleware);
        }

        /// <inheritdoc cref="IWorkflowMiddlewareRunner.RunPostMiddleware"/>
        public Task RunPostMiddleware(WorkflowInstance workflow, WorkflowDefinition def)
        {
            return RunWorkflowMiddlewareWithErrorHandling(
                workflow,
                WorkflowMiddlewarePhase.PostWorkflow,
                def.OnPostMiddlewareError);
        }

        /// <inheritdoc cref="IWorkflowMiddlewareRunner.RunExecuteMiddleware"/>
        public Task RunExecuteMiddleware(WorkflowInstance workflow, WorkflowDefinition def)
        {
            return RunWorkflowMiddlewareWithErrorHandling(
                workflow,
                WorkflowMiddlewarePhase.ExecuteWorkflow,
                def.OnExecuteMiddlewareError);
        }

        public async Task RunWorkflowMiddlewareWithErrorHandling(
            WorkflowInstance workflow,
            WorkflowMiddlewarePhase phase,
            Type middlewareErrorType)
        {
            var middleware = _middleware.Where(m => m.Phase == phase);

            try
            {
                await RunWorkflowMiddleware(workflow, middleware);
            }
            catch (Exception exception)
            {
                var errorHandlerType = middlewareErrorType ?? typeof(IWorkflowMiddlewareErrorHandler);

                using (var scope = serviceProvider.CreateScope())
                {
                    var typeInstance = scope.ServiceProvider.GetService(errorHandlerType);
                    if (typeInstance is IWorkflowMiddlewareErrorHandler handler)
                    {
                        await handler.HandleAsync(exception);
                    }
                }
            }
        }

        private static Task RunWorkflowMiddleware(
            WorkflowInstance workflow,
            IEnumerable<IWorkflowMiddleware> middlewareCollection)
        {
            return middlewareCollection
                .Reverse()
                .Aggregate(
                    NoopWorkflowDelegate,
                    (previous, middleware) => () => middleware.HandleAsync(workflow, previous))();
        }
    }
}
