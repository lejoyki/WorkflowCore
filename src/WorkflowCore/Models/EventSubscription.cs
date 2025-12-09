using System;

namespace WorkflowCore.Models
{
    public class EventSubscription
    {
        public string Id { get; set; }

        public string WorkflowId { get; set; }

        public int StepId { get; set; }

        public string ExecutionPointerId { get; set; }

        public string EventName { get; set; }

        public string EventKey { get; set; }

        /// <summary>
        /// 订阅生效时间
        /// </summary>
        public DateTime SubscribeAsOf { get; set; }

        public object SubscriptionData { get; set; }
    }
}
