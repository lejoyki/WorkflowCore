namespace WorkflowCore.Interface
{
    /// <summary>
    /// 后台任务接口，定义可以在后台运行的任务
    /// </summary>
    public interface IBackgroundTask
    {
        /// <summary>
        /// 启动后台任务
        /// </summary>
        void Start();
        
        /// <summary>
        /// 停止后台任务
        /// </summary>
        void Stop();
    }
}