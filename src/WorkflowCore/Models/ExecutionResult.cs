using System;
using System.Collections.Generic;
using WorkflowCore.Interface;

namespace WorkflowCore.Models
{
    /// <summary>
    /// 执行结果类，表示工作流步骤执行后的结果，包含控制工作流执行流程的各种信息
    /// </summary>
    public class ExecutionResult
    {
        /// <summary>
        /// 指示是否继续执行后续步骤的标志
        /// </summary>
        public bool Proceed { get; set; }

        /// <summary>
        /// 步骤执行的输出值，可以传递给后续步骤使用
        /// </summary>
        public object OutcomeValue { get; set; }

        /// <summary>
        /// 休眠时长，如果设置了该值，工作流将在指定时间后继续执行
        /// </summary>
        public TimeSpan? SleepFor { get; set; }

        /// <summary>
        /// 持久化数据，用于保存步骤的中间状态，以便在工作流重启后能够恢复
        /// </summary>
        public object PersistenceData { get; set; }

        /// <summary>
        /// 等待的事件名称，如果设置了该值，工作流将等待指定事件触发后继续执行
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// 等待的事件键，用于进一步指定等待的具体事件实例
        /// </summary>
        public string EventKey { get; set; }

        /// <summary>
        /// 事件生效时间，指定从什么时间开始监听事件
        /// </summary>
        public DateTime EventAsOf { get; set; }

        /// <summary>
        /// 订阅数据，用于传递给事件订阅的相关信息
        /// </summary>
        public object SubscriptionData { get; set; }

        /// <summary>
        /// 期望工作流状态，用于指示工作流应进入的特定状态
        /// </summary>
        public WorkflowStatus DesiredWorkflowStatus { get; set; }

        /// <summary>
        /// 分支值列表，用于创建并行执行分支
        /// </summary>
        public List<object> BranchValues { get; set; } = new List<object>();

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ExecutionResult()
        {
        }

        /// <summary>
        /// 使用输出值初始化执行结果
        /// </summary>
        /// <param name="outcome">步骤执行的输出值</param>
        public ExecutionResult(object outcome)
        {
            Proceed = true;
            OutcomeValue = outcome;
        }

        /// <summary>
        /// 创建包含输出值的执行结果，指示步骤成功完成并继续执行后续步骤
        /// </summary>
        /// <param name="value">步骤的输出值</param>
        /// <returns>执行结果对象</returns>
        public static ExecutionResult Outcome(object value)
        {
            return new ExecutionResult
            {
                Proceed = true,
                OutcomeValue = value
            };
        }

        /// <summary>
        /// 创建表示继续执行下一步的执行结果
        /// </summary>
        /// <returns>执行结果对象</returns>
        public static ExecutionResult Next()
        {
            return new ExecutionResult
            {
                Proceed = true,
                OutcomeValue = null
            };
        }

        /// <summary>
        /// 创建持久化执行结果，保存步骤状态但暂停执行
        /// </summary>
        /// <param name="persistenceData">需要持久化的数据</param>
        /// <returns>执行结果对象</returns>
        public static ExecutionResult Persist(object persistenceData)
        {
            return new ExecutionResult
            {
                Proceed = false,
                PersistenceData = persistenceData
            };
        }

        /// <summary>
        /// 创建分支执行结果，用于启动并行执行分支
        /// </summary>
        /// <param name="branches">分支值列表</param>
        /// <param name="persistenceData">持久化数据</param>
        /// <returns>执行结果对象</returns>
        public static ExecutionResult Branch(List<object> branches, object persistenceData)
        {
            return new ExecutionResult
            {
                Proceed = false,
                PersistenceData = persistenceData,
                BranchValues = branches
            };
        }

        /// <summary>
        /// 创建休眠执行结果，使工作流在指定时间后继续执行
        /// </summary>
        /// <param name="duration">休眠时长</param>
        /// <param name="persistenceData">持久化数据</param>
        /// <returns>执行结果对象</returns>
        public static ExecutionResult Sleep(TimeSpan duration, object persistenceData)
        {
            return new ExecutionResult
            {
                Proceed = false,
                SleepFor = duration,
                PersistenceData = persistenceData
            };
        }

        /// <summary>
        /// 创建等待事件的执行结果，使工作流等待指定事件触发后继续执行
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="eventKey">事件键</param>
        /// <param name="effectiveDate">生效日期</param>
        /// <returns>执行结果对象</returns>
        public static ExecutionResult WaitForEvent(string eventName, string eventKey, DateTime effectiveDate)
        {
            return new ExecutionResult
            {
                Proceed = false,
                EventName = eventName,
                EventKey = eventKey,
                EventAsOf = effectiveDate
            };
        }
        /// <summary>
        /// 重试当前流程步骤
        /// </summary>
        /// <returns></returns>
        public static ExecutionResult Retry(IStepExecutionContext context)
        {
            context.ExecutionPointer.RetryCount++;
            return new ExecutionResult
            {
                Proceed = false,
            };
        }

        /// <summary>
        /// 期望工作流进入指定状态的执行结果
        /// </summary>
        /// <param name="desiredStatus">期望工作流状态</param>
        /// <param name="proceed">跳过该步骤,进行下一步步骤</param>
        /// <returns>执行结果对象</returns>
        public static ExecutionResult DesiredStatus(WorkflowStatus desiredStatus, bool proceed)
        {
            return new ExecutionResult
            {
                Proceed = proceed,
                DesiredWorkflowStatus = desiredStatus,
            };
        }

        /// <summary>
        /// 暂停工作流
        /// </summary>
        /// <returns></returns>
        public static ExecutionResult Suspend(SuspendMode suspendMode = SuspendMode.Immediate)
        {
            return DesiredStatus(WorkflowStatus.Suspended, suspendMode == SuspendMode.WaitCurrentStepComplete);
        }

        /// <summary>
        /// 终止工作流
        /// </summary>
        /// <returns></returns>
        public static ExecutionResult Terminate()
        {
            return DesiredStatus(WorkflowStatus.Terminated, false);
        }

        /// <summary>
        /// 工作流完成
        /// </summary>
        /// <returns></returns>
        public static ExecutionResult Complete()
        {
            return DesiredStatus(WorkflowStatus.Complete, true);
        }
    }
}
