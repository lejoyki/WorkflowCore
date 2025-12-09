using System;
using System.Collections.Generic;
using WorkflowCore.Models;
using WorkflowCore.Primitives;

namespace WorkflowCore.Interface
{
    /// <summary>
    /// 工作流构建器接口，用于构建工作流定义
    /// </summary>
    public interface IWorkflowBuilder
    {
        /// <summary>
        /// 工作流步骤集合
        /// </summary>
        List<WorkflowStep> Steps { get; }

        /// <summary>
        /// 最后一个步骤的ID
        /// </summary>
        int LastStep { get; }

        /// <summary>
        /// 指定工作流数据类型
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>带类型的工作流构建器</returns>
        IWorkflowBuilder<T> UseData<T>();

        /// <summary>
        /// 构建工作流定义
        /// </summary>
        /// <param name="id">工作流ID</param>
        /// <param name="version">版本号</param>
        /// <returns>工作流定义</returns>
        WorkflowDefinition Build(string id, int version);

        /// <summary>
        /// 添加工作流步骤
        /// </summary>
        /// <param name="step">工作流步骤</param>
        void AddStep(WorkflowStep step);

        /// <summary>
        /// 附加分支构建器
        /// </summary>
        /// <param name="branch">分支构建器</param>
        void AttachBranch(IWorkflowBuilder branch);
    }

    /// <summary>
    /// 带数据类型的工作流构建器接口
    /// </summary>
    /// <typeparam name="TData">数据类型</typeparam>
    public interface IWorkflowBuilder<TData> : IWorkflowBuilder, IWorkflowModifier<TData, InlineStepBody>
    {
        /// <summary>
        /// 以指定步骤类型开始工作流
        /// </summary>
        /// <typeparam name="TStep">步骤类型</typeparam>
        /// <param name="stepSetup">步骤设置回调</param>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, TStep> StartWith<TStep>(Action<IStepBuilder<TData, TStep>>? stepSetup = null) where TStep : IStepBody;

        /// <summary>
        /// 以函数形式开始工作流
        /// </summary>
        /// <param name="body">步骤执行函数</param>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, InlineStepBody> StartWith(Func<IStepExecutionContext, ExecutionResult> body);

        /// <summary>
        /// 以动作形式开始工作流
        /// </summary>
        /// <param name="body">步骤执行动作</param>
        /// <returns>步骤构建器</returns>
        IStepBuilder<TData, ActionStepBody> StartWith(Action<IStepExecutionContext> body);

        /// <summary>
        /// 获取指定步骤的上游步骤
        /// </summary>
        /// <param name="id">步骤ID</param>
        /// <returns>上游步骤集合</returns>
        IEnumerable<WorkflowStep> GetUpstreamSteps(int id);

        /// <summary>
        /// 设置默认错误处理行为
        /// </summary>
        /// <param name="behavior">错误处理行为</param>
        /// <param name="retryInterval">重试间隔</param>
        /// <returns>工作流构建器</returns>
        IWorkflowBuilder<TData> UseDefaultErrorBehavior(WorkflowErrorHandling behavior, TimeSpan? retryInterval = null);

        /// <summary>
        /// 创建分支构建器
        /// </summary>
        /// <returns>分支构建器</returns>
        IWorkflowBuilder<TData> CreateBranch();
    }
}
