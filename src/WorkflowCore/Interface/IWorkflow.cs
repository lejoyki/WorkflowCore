namespace WorkflowCore.Interface
{
    /// <summary>
    /// 工作流定义接口，用于定义工作流的结构和逻辑
    /// </summary>
    /// <typeparam name="TData">工作流数据类型</typeparam>
    public interface IWorkflow<TData>
        where TData : class
    {
        /// <summary>
        /// 工作流的唯一标识符
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// 工作流版本号，用于支持工作流的版本控制
        /// </summary>
        int Version { get; }
        
        /// <summary>
        /// 构建工作流定义，通过构建器配置工作流的步骤和逻辑
        /// </summary>
        /// <param name="builder">工作流构建器</param>
        void Build(IWorkflowBuilder<TData> builder);
    }

    /// <summary>
    /// 使用 object 作为数据类型的工作流接口
    /// </summary>
    public interface IWorkflow : IWorkflow<object>
    {
    }
}
