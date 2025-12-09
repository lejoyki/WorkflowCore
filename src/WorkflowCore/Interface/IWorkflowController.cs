using System;
using System.Threading.Tasks;

namespace WorkflowCore.Interface
{
    /// <summary>
    /// 工作流控制器接口，提供工作流的启动、事件发布和注册功能
    /// </summary>
    public interface IWorkflowController
    {
        /// <summary>
        /// 启动工作流实例
        /// </summary>
        /// <param name="workflowName">工作流ID</param>
        /// <param name="data">工作流数据</param>
        /// <param name="reference">引用标识</param>
        /// <returns>工作流实例ID</returns>
        Task<string> StartWorkflow(string workflowName, object? data = null);

        /// <summary>
        /// 启动指定版本的工作流实例
        /// </summary>
        /// <param name="workflowName">工作流ID</param>
        /// <param name="version">工作流版本</param>
        /// <param name="data">工作流数据</param>
        /// <param name="reference">引用标识</param>
        /// <returns>工作流实例ID</returns>
        Task<string> StartWorkflow(string workflowName, int? version, object? data = null);

        /// <summary>
        /// 启动带类型数据的工作流实例
        /// </summary>
        /// <typeparam name="TData">数据类型</typeparam>
        /// <param name="workflowName">工作流ID</param>
        /// <param name="data">工作流数据</param>
        /// <param name="reference">引用标识</param>
        /// <returns>工作流实例ID</returns>
        Task<string> StartWorkflow<TData>(string workflowName, TData? data = null) where TData : class;

        /// <summary>
        /// 启动指定版本带类型数据的工作流实例
        /// </summary>
        /// <typeparam name="TData">数据类型</typeparam>
        /// <param name="workflowName">工作流ID</param>
        /// <param name="version">工作流版本</param>
        /// <param name="data">工作流数据</param>
        /// <param name="reference">引用标识</param>
        /// <returns>工作流实例ID</returns>
        Task<string> StartWorkflow<TData>(string workflowName, int? version, TData? data = null) where TData : class;

        /// <summary>
        /// 发布工作流事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="eventKey">事件键</param>
        /// <param name="eventData">事件数据</param>
        /// <param name="effectiveDate">生效时间</param>
        Task PublishEvent(string eventName, string eventKey, object eventData, DateTime? effectiveDate = null);
        
        /// <summary>
        /// 注册工作流定义
        /// </summary>
        /// <typeparam name="TWorkflow">工作流类型</typeparam>
        void RegisterWorkflow<TWorkflow>() where TWorkflow : IWorkflow;
        
        /// <summary>
        /// 注册带数据类型的工作流定义
        /// </summary>
        /// <typeparam name="TWorkflow">工作流类型</typeparam>
        /// <typeparam name="TData">数据类型</typeparam>
        void RegisterWorkflow<TWorkflow, TData>() where TWorkflow : IWorkflow<TData> where TData : class;

        /// <summary>
        /// 暂停工作流的执行，直到调用 ResumeWorkflow 方法恢复
        /// </summary>
        /// <param name="workflowId">工作流实例ID</param>
        /// <returns>是否成功暂停</returns>
        Task<bool> SuspendWorkflow(string workflowId);

        /// <summary>
        /// 恢复之前暂停的工作流
        /// </summary>
        /// <param name="workflowId">工作流实例ID</param>
        /// <returns>是否成功恢复</returns>
        Task<bool> ResumeWorkflow(string workflowId);

        /// <summary>
        /// 永久终止指定工作流的执行
        /// </summary>
        /// <param name="workflowId">工作流实例ID</param>
        /// <returns>是否成功终止</returns>
        Task<bool> TerminateWorkflow(string workflowId);

    }
}
