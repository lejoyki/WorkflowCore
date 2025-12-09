using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCore.Primitives;

public sealed class NullStepBody : StepBody
{
    public override ExecutionResult Run(IStepExecutionContext context)
    {
        return ExecutionResult.Next();
    }
}