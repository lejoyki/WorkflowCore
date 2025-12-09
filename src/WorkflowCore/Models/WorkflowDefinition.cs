using System;


namespace WorkflowCore.Models
{
    /// <summary>
    /// 工作流定义类，描述工作流的结构、步骤和配置信息
    /// </summary>
    public class WorkflowDefinition
    {
        public string Name { get; set; }

        /// <summary>
        /// 工作流定义的版本号
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 工作流步骤集合
        /// </summary>
        public WorkflowStepCollection Steps { get; set; } = new WorkflowStepCollection();

        /// <summary>
        /// 工作流数据类型
        /// </summary>
        public Type DataType { get; set; }

        /// <summary>
        /// 默认错误处理行为，定义当工作流步骤发生错误时的处理策略
        /// </summary>
        public WorkflowErrorHandling DefaultErrorBehavior { get; set; }

        /// <summary>
        /// 后置中间件错误处理器类型，用于处理后置中间件执行时的错误
        /// </summary>
        public Type OnPostMiddlewareError { get; set; }
        
        /// <summary>
        /// 执行中间件错误处理器类型，用于处理执行中间件时的错误
        /// </summary>
        public Type OnExecuteMiddlewareError { get; set; }

        /// <summary>
        /// 默认错误重试间隔时间，定义发生错误后重试的等待时间
        /// </summary>
        public TimeSpan? DefaultErrorRetryInterval { get; set; }
    }

    /// <summary>
    /// 工作流错误处理枚举，定义工作流执行过程中遇到错误时的处理策略
    /// </summary>
    public enum WorkflowErrorHandling
    {
        /// <summary>
        /// 重试策略，发生错误时重新执行失败的步骤
        /// </summary>
        Retry = 0,
        /// <summary>
        /// 暂停策略，发生错误时暂停工作流执行，等待人工干预
        /// </summary>
        Suspend = 1,
        /// <summary>
        /// 终止策略，发生错误时立即终止工作流执行
        /// </summary>
        Terminate = 2
    }
}
