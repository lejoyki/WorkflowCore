namespace WorkflowCore.Models;

public enum SuspendMode
{
    /// <summary>
    /// 立即挂起，立即进入暂停状态,重新启动后继续当前步骤
    /// </summary>
    Immediate,
    /// <summary>
    /// 等待当前步骤完成后挂起，当前步骤完成后进入暂停状态,重新启动后执行下一步骤
    /// </summary>
    WaitCurrentStepComplete
}