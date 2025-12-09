using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Hosting;
using WorkflowCore.Models;
using WorkflowCore.Models.LifeCycleEvents;

namespace WorkflowCore.Interface
{
    /// <summary>
    /// 工作流主机接口，负责管理工作流引擎的生命周期和执行环境
    /// </summary>
    public interface IWorkflowHost : IWorkflowController, IHostedService
    {
        /// <summary>
        /// 启动工作流主机，开启工作流的执行
        /// </summary>
        void Start();

        /// <summary>
        /// 停止工作流主机
        /// </summary>
        void Stop();
        
        /// <summary>
        /// 步骤错误事件
        /// </summary>
        event StepErrorEventHandler OnStepError;
        
        /// <summary>
        /// 生命周期事件
        /// </summary>
        event LifeCycleEventHandler OnLifeCycleEvent;
        
        /// <summary>
        /// 报告步骤执行错误
        /// </summary>
        /// <param name="workflow">工作流实例</param>
        /// <param name="step">出错的步骤</param>
        /// <param name="exception">异常信息</param>
        void ReportStepError(WorkflowInstance workflow, WorkflowStep step, Exception exception);

        /// <summary>
        /// 持久化存储提供程序，用于扩展方法访问
        /// </summary>
        IPersistenceProvider PersistenceStore { get; }
        
        /// <summary>
        /// 工作流注册表，用于扩展方法访问
        /// </summary>
        IWorkflowRegistry Registry { get; }
        
        /// <summary>
        /// 工作流选项配置，用于扩展方法访问
        /// </summary>
        WorkflowOptions Options { get; }
        
        /// <summary>
        /// 队列提供程序，用于扩展方法访问
        /// </summary>
        IQueueProvider QueueProvider { get; }
        
        /// <summary>
        /// 日志记录器，用于扩展方法访问
        /// </summary>
        ILogger Logger { get; }

    }

    public delegate void StepErrorEventHandler(WorkflowInstance workflow, WorkflowStep step, Exception exception);
    public delegate void LifeCycleEventHandler(LifeCycleEvent evt);
}