using WorkflowCore.Models;

namespace WorkflowCore.Interface
{
    public interface IExecutionPointerFactory
    {
        ExecutionPointer BuildGenesisPointer(WorkflowDefinition def);
        ExecutionPointer BuildNextPointer(WorkflowDefinition def, ExecutionPointer pointer, IStepOutcome outcomeTarget);
        ExecutionPointer BuildChildPointer(WorkflowDefinition def, ExecutionPointer pointer, int childDefinitionId, object branch);
    }
}