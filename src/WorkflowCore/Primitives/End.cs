using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCore.Primitives
{
    public sealed class End : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            return ExecutionResult.Complete();
        }
    }
}
