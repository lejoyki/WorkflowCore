using System.Threading.Tasks;
using WorkflowCore.Models;

namespace WorkflowCore.Interface
{
    /// <summary>
    /// 步骤执行器接口，负责执行工作流步骤
    /// </summary>
    public interface IStepExecutor
    {
        /// <summary>
        /// 在指定的执行上下文中运行步骤体
        /// </summary>
        /// <param name="context">步骤执行上下文</param>
        /// <param name="body">步骤执行体</param>
        /// <returns>等待步骤执行结果的任务</returns>
        Task<ExecutionResult> ExecuteStep(
            IStepExecutionContext context,
            IStepBody body
        );
    }
}
