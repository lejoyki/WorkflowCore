using System;

namespace WorkflowCore.Models.LifeCycleEvents
{
    public abstract class LifeCycleEvent
    {
        public WorkflowStatus WorkflowStatus { get; set; }

        public DateTime EventTime { get; set; }

        public required string WorkflowInstanceId { get; set; }

        public required string WorkflowName { get; set; }

        public int Version { get; set; }
    }
}
