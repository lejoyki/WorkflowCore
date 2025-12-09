using System.Threading.Tasks;
using WorkflowCore.Models;

namespace WorkflowCore.Interface
{
    /// <summary>
    /// 运行工作流前置/后置和执行中间件。
    /// </summary>
    public interface IWorkflowMiddlewareRunner
    {
        /// <summary>
        /// 运行设置为在 <see cref="WorkflowMiddlewarePhase.PreWorkflow"/> 阶段执行的工作流级别中间件
        /// </summary>
        /// <param name="workflow">要运行的 <see cref="WorkflowInstance"/> 工作流实例</param>
        /// <param name="def">工作流定义 <see cref="WorkflowDefinition"/></param>
        /// <returns></returns>
        Task RunPreMiddleware(WorkflowInstance workflow, WorkflowDefinition def);

        /// <summary>
        /// 运行设置为在 <see cref="WorkflowMiddlewarePhase.PostWorkflow"/> 阶段执行的工作流级别中间件
        /// </summary>
        /// <param name="workflow">要运行的 <see cref="WorkflowInstance"/> 工作流实例</param>
        /// <param name="def">工作流定义 <see cref="WorkflowDefinition"/></param>
        /// <returns></returns>
        Task RunPostMiddleware(WorkflowInstance workflow, WorkflowDefinition def);

        /// <summary>
        /// 运行设置为在 <see cref="WorkflowMiddlewarePhase.ExecuteWorkflow"/> 阶段执行的工作流级别中间件
        /// </summary>
        /// <param name="workflow">要运行的 <see cref="WorkflowInstance"/> 工作流实例</param>
        /// <param name="def">工作流定义 <see cref="WorkflowDefinition"/></param>
        /// <returns></returns>
        Task RunExecuteMiddleware(WorkflowInstance workflow, WorkflowDefinition def);
    }
}
