using System;
using System.Threading.Tasks;
using WorkflowCore.Interface;

namespace WorkflowCore.Models
{
    public abstract class StepBodyAsync : IStepBody
    {
        public virtual ValueTask AfterRunAsync(IStepExecutionContext context)
        {
            return new ValueTask();
        }

        public virtual ValueTask BeforRunAsync(IStepExecutionContext context)
        {
            return new ValueTask();
        }

        public abstract Task<ExecutionResult> RunAsync(IStepExecutionContext context);
    }
}
