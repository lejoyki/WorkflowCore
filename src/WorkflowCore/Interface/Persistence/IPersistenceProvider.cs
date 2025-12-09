namespace WorkflowCore.Interface
{
    /// <summary>
    /// 持久化提供程序接口，集成了工作流、订阅、事件和计划命令的存储库功能
    /// </summary>
    public interface IPersistenceProvider : IWorkflowRepository, ISubscriptionRepository, IEventRepository
    {        
    }
}
