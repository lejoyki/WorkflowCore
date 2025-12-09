using System;
using System.Linq.Expressions;
using WorkflowCore.Models;
using WorkflowCore.Primitives;

namespace WorkflowCore.Interface
{
    /// <summary>
    /// 步骤构建器接口，用于配置工作流中的单个步骤
    /// </summary>
    /// <typeparam name="TData">工作流数据类型</typeparam>
    /// <typeparam name="TStepBody">步骤体类型</typeparam>
    public interface IStepBuilder<TData, TStepBody> : IWorkflowModifier<TData, TStepBody>
        where TStepBody : IStepBody
    {

        /// <summary>
        /// 所属的工作流构建器
        /// </summary>
        IWorkflowBuilder<TData> WorkflowBuilder { get; }        

        /// <summary>
        /// 当前步骤实例
        /// </summary>
        WorkflowStep<TStepBody> Step { get; set; }

        /// <summary>
        /// 指定步骤的显示名称，用于日志等场景的便捷识别
        /// </summary>
        /// <param name="name">步骤的显示名称</param>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, TStepBody> Name(string name);

        /// <summary>
        /// 指定用于引用此步骤的自定义ID
        /// </summary>
        /// <param name="id">用于引用此步骤的自定义ID</param>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, TStepBody> ExternalId(string id);

        /// <summary>
        /// 附加另一个已存在的步骤作为此步骤的后续步骤
        /// </summary>
        /// <param name="id">要附加的步骤External ID</param>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, TStepBody> Attach(string id);

        /// <summary>
        /// 为此步骤配置结果分支，然后将其链接到另一个步骤
        /// </summary>
        /// <typeparam name="TStep">分支步骤类型</typeparam>
        /// <param name="outcomeValue">结果值</param>
        /// <param name="branch">分支步骤构建器</param>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, TStepBody> Branch<TStep>(object outcomeValue, IStepBuilder<TData, TStep> branch) where TStep : IStepBody;

        /// <summary>
        /// 使用表达式为此步骤配置结果分支，然后将其链接到另一个步骤
        /// </summary>
        /// <typeparam name="TStep">分支步骤类型</typeparam>
        /// <param name="outcomeExpression">结果表达式</param>
        /// <param name="branch">分支步骤构建器</param>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, TStepBody> Branch<TStep>(Expression<Func<TData, object, bool>> outcomeExpression, IStepBuilder<TData, TStep> branch) where TStep : IStepBody;

        /// <summary>
        /// 在步骤执行前，将工作流数据对象的属性映射到步骤的属性
        /// </summary>
        /// <typeparam name="TInput">输入类型</typeparam>
        /// <param name="stepProperty">步骤属性</param>
        /// <param name="value">数据属性表达式</param>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, TStepBody> Input<TInput>(Expression<Func<TStepBody, TInput>> stepProperty, Expression<Func<TData, TInput>> value);

        /// <summary>
        /// 在步骤执行前，使用带上下文的表达式将数据映射到步骤的属性
        /// </summary>
        /// <typeparam name="TInput">输入类型</typeparam>
        /// <param name="stepProperty">步骤属性</param>
        /// <param name="value">带上下文的数据表达式</param>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, TStepBody> Input<TInput>(Expression<Func<TStepBody, TInput>> stepProperty, Expression<Func<TData, IStepExecutionContext, TInput>> value);

        /// <summary>
        /// 在步骤执行前操作步骤的属性
        /// </summary>
        /// <param name="action">操作动作</param>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, TStepBody> Input(Action<TStepBody, TData> action);
        
        /// <summary>
        /// 在步骤执行前使用上下文操作步骤的属性
        /// </summary>
        /// <param name="action">操作动作</param>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, TStepBody> Input(Action<TStepBody, TData, IStepExecutionContext> action);

        /// <summary>
        /// 在步骤执行后，将步骤的属性映射到工作流数据对象的属性
        /// </summary>
        /// <typeparam name="TOutput">输出类型</typeparam>
        /// <param name="dataProperty">数据对象属性</param>
        /// <param name="value">步骤属性表达式</param>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, TStepBody> Output<TOutput>(Expression<Func<TData, TOutput>> dataProperty, Expression<Func<TStepBody, object>> value);

        /// <summary>
        /// 在步骤执行后操作数据对象的属性
        /// </summary>
        /// <param name="action">操作动作</param>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, TStepBody> Output(Action<TStepBody, TData> action);

        /// <summary>
        /// 配置此步骤抛出未处理异常时的行为
        /// </summary>
        /// <param name="behavior">异常处理行为</param>
        /// <param name="retryInterval">如果行为是重试，指定重试间隔</param>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, TStepBody> OnError(WorkflowErrorHandling behavior, TimeSpan? retryInterval = null);

        /// <summary>
        /// 在满足条件时提前取消此步骤的执行
        /// </summary>
        /// <param name="cancelCondition">取消条件</param>
        /// <param name="proceedAfterCancel">取消后是否继续执行</param>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, TStepBody> CancelCondition(Expression<Func<TData, bool>> cancelCondition, bool proceedAfterCancel = false);
    }
}
