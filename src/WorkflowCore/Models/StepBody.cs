using System;
using System.Threading.Tasks;
using WorkflowCore.Interface;

namespace WorkflowCore.Models
{
    public abstract class StepBody : IStepBody
    {
        public virtual ValueTask AfterRunAsync(IStepExecutionContext context)
        {
            return new ValueTask();
        }

        public ValueTask BeforRunAsync(IStepExecutionContext context)
        {
            return new ValueTask();
        }

        public abstract ExecutionResult Run(IStepExecutionContext context);

        public Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            return Task.FromResult(Run(context));
        }
    }
}
