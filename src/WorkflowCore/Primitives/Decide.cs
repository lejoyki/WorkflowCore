using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCore.Primitives
{
    public sealed class Decide : StepBody
    {
        public object Expression { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            return ExecutionResult.Outcome(Expression);
        }
    }
}
