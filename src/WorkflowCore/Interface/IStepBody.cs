using System.Threading.Tasks;
using WorkflowCore.Models;

namespace WorkflowCore.Interface
{
    /// <summary>
    /// 工作流步骤执行体接口，定义步骤的具体执行逻辑
    /// </summary>
    public interface IStepBody
    {
        ValueTask BeforRunAsync(IStepExecutionContext context);

        /// <summary>
        /// 异步执行步骤逻辑
        /// </summary>
        /// <param name="context">步骤执行上下文</param>
        /// <returns>执行结果</returns>
        Task<ExecutionResult> RunAsync(IStepExecutionContext context);


        ValueTask AfterRunAsync(IStepExecutionContext context);
    }
}
