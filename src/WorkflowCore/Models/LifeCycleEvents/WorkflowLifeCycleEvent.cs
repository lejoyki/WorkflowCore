using System.Diagnostics;

namespace WorkflowCore.Models.LifeCycleEvents;

public class WorkflowLifeCycleEvent : LifeCycleEvent
{
    public required DateTime CreatTime { get; set; }

    public required DateTime? CompleteTime { get; set; }

    public override string ToString()
    {
        var statusDescription = WorkflowStatus switch
        {
            WorkflowStatus.Runnable => "已启动",
            WorkflowStatus.Suspended => "已暂停",
            WorkflowStatus.Complete => "已完成",
            WorkflowStatus.Terminated => "被终止",
            _ => "Unknown"
        };

        var msg = $"流程[{WorkflowName}] {statusDescription}";
        
        if(WorkflowStatus == WorkflowStatus.Complete && CompleteTime.HasValue)
        {
            var duration = CompleteTime.Value - CreatTime;
            msg += $", 持续时间: {duration.TotalSeconds:F1} s";
        }
        
        return msg;
    }
}