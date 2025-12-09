using WorkflowCore.Models;

namespace WorkflowCore.Interface
{
    public interface IStepOutcome
    {
        string? ExternalNextStepId { get; set; }
        int NextStep { get; set; }

        bool Matches(object data);
        bool Matches(ExecutionResult executionResult, object data);
    }
}