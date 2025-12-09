using System;
using System.Collections.Generic;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCore.Services.ErrorHandlers
{
    public class RetryHandler : IWorkflowErrorHandler
    {
        private readonly WorkflowOptions _options;
        public WorkflowErrorHandling Type => WorkflowErrorHandling.Retry;

        public RetryHandler(WorkflowOptions options)
        {
            _options = options;
        }

        public void Handle(WorkflowInstance workflow, WorkflowDefinition def, ExecutionPointer pointer, WorkflowStep step, Exception exception, Queue<ExecutionPointer> bubbleUpQueue)
        {
            pointer.RetryCount++;
            pointer.SleepUntil = DateTime.Now.Add(step.RetryInterval ?? def.DefaultErrorRetryInterval ?? _options.ErrorRetryInterval);
        }
    }
}
