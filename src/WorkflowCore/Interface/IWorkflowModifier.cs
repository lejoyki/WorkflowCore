using System;
using System.Collections;
using System.Linq.Expressions;
using WorkflowCore.Models;
using WorkflowCore.Primitives;

namespace WorkflowCore.Interface
{
    /// <summary>
    /// 工作流修改器接口，提供构建工作流的链式调用方法
    /// </summary>
    /// <typeparam name="TData">工作流数据类型</typeparam>
    /// <typeparam name="TStepBody">步骤体类型</typeparam>
    public interface IWorkflowModifier<TData, TStepBody>
        where TStepBody : IStepBody
    {
        /// <summary>
        /// 指定工作流中的下一个步骤
        /// </summary>
        /// <typeparam name="TStep">要执行的步骤类型</typeparam>
        /// <param name="stepSetup">为此步骤配置额外参数</param>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, TStep> Then<TStep>(Action<IStepBuilder<TData, TStep>>? stepSetup = null) where TStep : IStepBody;

        /// <summary>
        /// 指定工作流中的下一个步骤
        /// </summary>
        /// <typeparam name="TStep">步骤类型</typeparam>
        /// <param name="newStep">新步骤实例</param>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, TStep> Then<TStep>(IStepBuilder<TData, TStep> newStep) where TStep : IStepBody;

        /// <summary>
        /// 指定工作流中的内联下一步骤（使用函数）
        /// </summary>
        /// <param name="body">步骤执行函数</param>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, InlineStepBody> Then(Func<IStepExecutionContext, ExecutionResult> body);

        /// <summary>
        /// 指定工作流中的内联下一步骤（使用函数）
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        IStepBuilder<TData,InlineAsyncStepBody> Then(Func<IStepExecutionContext,Task<ExecutionResult>> body);

        /// <summary>
        /// 指定工作流中的内联下一步骤（使用动作）
        /// </summary>
        /// <param name="body">步骤执行动作</param>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, ActionStepBody> Then(Action<IStepExecutionContext> body);

        /// <summary>
        /// 等待直到指定事件被发布
        /// </summary>
        /// <param name="eventName">用于标识等待事件类型的名称</param>
        /// <param name="eventKey">事件上下文中的特定键值</param>
        /// <param name="effectiveDate">监听事件的生效日期</param>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, WaitFor> WaitFor(string eventName, Expression<Func<TData, string>> eventKey,
            Expression<Func<TData, DateTime>> effectiveDate = null);

        /// <summary>
        /// 等待直到指定事件被发布（带步骤上下文的重载）
        /// </summary>
        /// <param name="eventName">用于标识等待事件类型的名称</param>
        /// <param name="eventKey">事件上下文中的特定键值（带步骤上下文）</param>
        /// <param name="effectiveDate">监听事件的生效日期</param>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, WaitFor> WaitFor(string eventName,
            Expression<Func<TData, IStepExecutionContext, string>> eventKey,
            Expression<Func<TData, DateTime>> effectiveDate = null);

        /// <summary>
        /// 等待指定时间段
        /// </summary>
        /// <param name="period">等待的时间段</param>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, Delay> Delay(Expression<Func<TData, TimeSpan>> period);
        
        /// <summary>
        /// 跳转步骤
        /// </summary>
        /// <param name="externalId">跳转步骤的扩展id</param>
        void JumpTo(string externalId);

        /// <summary>
        /// 计算表达式并根据值选择不同的执行路径
        /// </summary>
        /// <param name="expression">用于决策的表达式</param>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, Decide> Decide(Expression<Func<TData, object>> expression);

        /// <summary>
        /// 对集合中的每个项目执行一组步骤（并行 foreach）
        /// </summary>
        /// <param name="collection">要迭代的集合</param>
        /// <returns>容器步骤构建器</returns>
        IContainerStepBuilder<TData, Foreach, Foreach> ForEach(Expression<Func<TData, IEnumerable>> collection);

        /// <summary>
        /// 对集合中的每个项目执行一组步骤（可配置并行执行）
        /// </summary>
        /// <param name="collection">要迭代的集合</param>
        /// <param name="runParallel">是否并行运行</param>
        /// <returns>容器步骤构建器</returns>
        IContainerStepBuilder<TData, Foreach, Foreach> ForEach(Expression<Func<TData, IEnumerable>> collection, Expression<Func<TData, bool>> runParallel);

        /// <summary>
        /// 对集合中的每个项目执行一组步骤（带步骤上下文，可配置并行执行）
        /// </summary>
        /// <param name="collection">要迭代的集合（带步骤上下文）</param>
        /// <param name="runParallel">是否并行运行</param>
        /// <returns>容器步骤构建器</returns>
        IContainerStepBuilder<TData, Foreach, Foreach> ForEach(Expression<Func<TData, IStepExecutionContext, IEnumerable>> collection, Expression<Func<TData, bool>> runParallel);

        /// <summary>
        /// 重复执行一组步骤，直到条件为真
        /// </summary>
        /// <param name="condition">跳出 while 循环的条件</param>
        /// <returns>容器步骤构建器</returns>
        IContainerStepBuilder<TData, While, While> While(Expression<Func<TData, bool>> condition);

        /// <summary>
        /// 重复执行一组步骤，直到条件为真（带步骤上下文）
        /// </summary>
        /// <param name="condition">跳出 while 循环的条件（带步骤上下文）</param>
        /// <returns>容器步骤构建器</returns>
        IContainerStepBuilder<TData, While, While> While(Expression<Func<TData, IStepExecutionContext, bool>> condition);

        /// <summary>
        /// 如果条件为真，则执行一组步骤
        /// </summary>
        /// <param name="condition">要评估的条件</param>
        /// <returns>容器步骤构建器</returns>
        IContainerStepBuilder<TData, If, If> If(Expression<Func<TData, bool>> condition);

        /// <summary>
        /// 如果条件为真，则执行一组步骤（带步骤上下文）
        /// </summary>
        /// <param name="condition">要评估的条件（带步骤上下文）</param>
        /// <returns>容器步骤构建器</returns>
        IContainerStepBuilder<TData, If, If> If(Expression<Func<TData, IStepExecutionContext, bool>> condition);

        /// <summary>
        /// 为此步骤配置一个结果值，然后连接到一个序列
        /// </summary>
        /// <param name="outcomeValue">结果值</param>
        /// <returns>容器步骤构建器</returns>
        IContainerStepBuilder<TData, When, OutcomeSwitch> When(Expression<Func<TData, object>> outcomeValue);

        /// <summary>
        /// 并行执行多个步骤块
        /// </summary>
        /// <returns>并行步骤构建器</returns>
        IParallelStepBuilder<TData, Sequence> Parallel();

        /// <summary>
        /// 结束工作流并标记为完成
        /// </summary>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, End> EndWorkflow();

        /// <summary>
        /// 计划在未来某个时间并行执行一组步骤
        /// </summary>
        /// <param name="time">执行前等待的时间跨度</param>
        /// <returns>容器步骤构建器</returns>
        IContainerStepBuilder<TData, Schedule, TStepBody> Schedule(Expression<Func<TData, TimeSpan>> time);

        /// <summary>
        /// 计划在未来以重复间隔并行执行一组步骤
        /// </summary>
        /// <param name="interval">重复执行之间等待的时间跨度</param>
        /// <param name="until">停止重复任务的条件</param>
        /// <returns>容器步骤构建器</returns>
        IContainerStepBuilder<TData, Recur, TStepBody> Recur(Expression<Func<TData, TimeSpan>> interval,
            Expression<Func<TData, bool>> until);
    }
}