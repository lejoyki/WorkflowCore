namespace WorkflowCore.Models.LifeCycleEvents;

public class WorkStepLifeCycleEvent : LifeCycleEvent
{
    public required string ExecutionPointerId { get; set; }

    public int StepId { get; set; }
    
    public required string? StepName { get; set; }
    
    public bool IsCompleted { get; set; }
    
    public TimeSpan Elapsed { get; set; }

    public override string ToString()
    {
        if (IsCompleted)
        {
            return $"[{StepName}]完成, 用时: {Elapsed.TotalSeconds:F1}s";
        }
        else
        {
            return $"[{StepName}]开始";
        }
    }
}