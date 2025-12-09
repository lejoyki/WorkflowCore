using System.Collections.Generic;
using WorkflowCore.Models;

namespace WorkflowCore.Interface
{
    /// <summary>
    /// 工作流注册表接口，用于管理工作流定义的注册和查询
    /// </summary>
    public interface IWorkflowRegistry
    {
        /// <summary>
        /// 注册工作流
        /// </summary>
        /// <param name="workflow">工作流实例</param>
        void RegisterWorkflow(IWorkflow workflow);
        
        /// <summary>
        /// 注册工作流定义
        /// </summary>
        /// <param name="definition">工作流定义</param>
        void RegisterWorkflow(WorkflowDefinition definition);
        
        /// <summary>
        /// 注册带数据类型的工作流
        /// </summary>
        /// <typeparam name="TData">数据类型</typeparam>
        /// <param name="workflow">工作流实例</param>
        void RegisterWorkflow<TData>(IWorkflow<TData> workflow) where TData : class;
        
        /// <summary>
        /// 获取工作流定义
        /// </summary>
        /// <param name="workflowName">工作流ID</param>
        /// <param name="version">版本号</param>
        /// <returns>工作流定义</returns>
        WorkflowDefinition GetDefinition(string workflowName, int? version = null);
        
        /// <summary>
        /// 检查工作流是否已注册
        /// </summary>
        /// <param name="workflowName">工作流ID</param>
        /// <param name="version">版本号</param>
        /// <returns>是否已注册</returns>
        bool IsRegistered(string workflowName, int version);
        
        /// <summary>
        /// 注销工作流
        /// </summary>
        /// <param name="workflowName">工作流ID</param>
        /// <param name="version">版本号</param>
        void DeregisterWorkflow(string workflowName, int version);
        
        /// <summary>
        /// 获取所有工作流定义
        /// </summary>
        /// <returns>工作流定义集合</returns>
        IEnumerable<WorkflowDefinition> GetAllDefinitions();
    }
}
