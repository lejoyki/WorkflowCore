using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCore.Primitives
{
    public sealed class WorkflowStepInline : WorkflowStep<InlineStepBody>
    {
        public Func<IStepExecutionContext, ExecutionResult>? Body { get; set; }

        public override IStepBody ConstructBody(IServiceProvider serviceProvider)
        {
            return new InlineStepBody(Body!);
        }
    }

    public sealed class WorkflowStepInlineAsync : WorkflowStep<InlineAsyncStepBody>
    {
        public Func<IStepExecutionContext, Task<ExecutionResult>>? Body { get; set; }
        public override IStepBody ConstructBody(IServiceProvider serviceProvider)
        {
            return new InlineAsyncStepBody(Body!);
        }
    }
}
