using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkflowCore.Models;

namespace WorkflowCore.Interface
{
    /// <summary>
    /// 工作流存储库接口，提供工作流实例的持久化操作
    /// </summary>
    public interface IWorkflowRepository
    {
        /// <summary>
        /// 创建新的工作流实例
        /// </summary>
        /// <param name="workflow">工作流实例</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>工作流实例ID</returns>
        Task<string> CreateNewWorkflow(WorkflowInstance workflow, CancellationToken cancellationToken = default);

        /// <summary>
        /// 持久化工作流实例
        /// </summary>
        /// <param name="workflow">工作流实例</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>异步任务</returns>
        Task PersistWorkflow(WorkflowInstance workflow, CancellationToken cancellationToken = default);

        /// <summary>
        /// 持久化工作流实例及其事件订阅
        /// </summary>
        /// <param name="workflow">工作流实例</param>
        /// <param name="subscriptions">事件订阅列表</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>异步任务</returns>
        Task PersistWorkflow(WorkflowInstance workflow, List<EventSubscription> subscriptions, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取可运行的工作流实例ID列表
        /// </summary>
        /// <param name="asAt">截止时间</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>可运行实例ID集合</returns>
        Task<IEnumerable<string>> GetRunnableInstances(DateTime asAt, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据ID获取工作流实例
        /// </summary>
        /// <param name="Id">工作流实例ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>工作流实例</returns>
        Task<WorkflowInstance?> GetWorkflowInstance(string Id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据workflowDefinitionId,获取id列表
        /// </summary>
        /// <param name="workflowName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>工作流实例id列表</returns>
        Task<List<WorkflowInstance>> FindWorkflowByDefinitionId(string workflowName, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据ID列表获取工作流实例
        /// </summary>
        /// <param name="ids">实例ID集合</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>工作流实例集合</returns>
        Task<IEnumerable<WorkflowInstance>> GetWorkflowInstances(IEnumerable<string> ids, CancellationToken cancellationToken = default);

    }
}
