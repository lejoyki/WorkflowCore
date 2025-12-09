using System.Threading;
using WorkflowCore.Models;

namespace WorkflowCore.Interface
{
    /// <summary>
    /// 步骤执行上下文接口，提供步骤执行期间需要的上下文信息
    /// </summary>
    public interface IStepExecutionContext
    {
        /// <summary>
        /// 当前处理的项目（在 ForEach 循环中使用）
        /// </summary>
        object Item { get; set; }

        /// <summary>
        /// 执行指针，标识当前步骤的执行位置
        /// </summary>
        ExecutionPointer ExecutionPointer { get; set; }

        /// <summary>
        /// 持久化数据，用于存储步骤间的状态信息
        /// </summary>
        object PersistenceData { get; set; }

        /// <summary>
        /// 当前执行的工作流步骤
        /// </summary>
        WorkflowStep Step { get; set; }

        /// <summary>
        /// 当前的工作流实例
        /// </summary>
        WorkflowInstance Workflow { get; set; }

        /// <summary>
        /// 取消令牌，用于取消步骤执行
        /// </summary>
        CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// 获取工作流数据上下文
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <returns></returns>
        TData GetData<TData>();
    }
}