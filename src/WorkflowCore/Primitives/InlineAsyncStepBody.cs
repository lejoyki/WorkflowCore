using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCore.Primitives
{
    public sealed class InlineAsyncStepBody : StepBodyAsync
    {
        private readonly Func<IStepExecutionContext,Task<ExecutionResult>> _body;

        public InlineAsyncStepBody(Func<IStepExecutionContext, Task<ExecutionResult>> body)
        {
            _body = body;
        }

        public override Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            return _body.Invoke(context);
        }
    }
}
