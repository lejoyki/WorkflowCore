using System.Threading.Tasks;
using WorkflowCore.Models;

namespace WorkflowCore.Interface
{
    /// <summary>
    /// 工作流步骤中间件接口,运行在工作流步骤周围,可以增强或改变步骤的行为
    /// </summary>
    public interface IWorkflowStepMiddleware
    {
        /// <summary>
        /// 处理工作流步骤并异步返回执行结果
        /// </summary>
        /// <param name="context">步骤的上下文</param>
        /// <param name="body">将要运行的步骤体实例</param>
        /// <param name="next">链中的下一个中间件</param>
        /// <returns>工作流执行结果的任务</returns>
        Task<ExecutionResult> HandleAsync(
            IStepExecutionContext context,
            IStepBody body,
            WorkflowStepDelegate next
        );
    }
}
