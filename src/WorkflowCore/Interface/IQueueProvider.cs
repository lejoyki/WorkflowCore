using System;
using System.Threading;
using System.Threading.Tasks;

namespace WorkflowCore.Interface
{
    /// <summary>
    /// 队列提供程序接口
    /// </summary>
    public interface IQueueProvider : IDisposable
    {

        /// <summary>
        /// 将工作加入队列，由集群中的主机进行处理
        /// </summary>
        /// <param name="id">工作项ID</param>
        /// <param name="queue">队列类型</param>
        /// <returns>异步任务</returns>
        Task QueueWork(string id, QueueType queue);

        /// <summary>
        /// 从处理队列的前端获取下一个工作项。如果队列为空，则返回 NULL
        /// </summary>
        /// <param name="queue">队列类型</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>工作项ID</returns>
        Task<string> DequeueWork(QueueType queue, CancellationToken cancellationToken);

        /// <summary>
        /// 获取一个值，指示出队操作是否为阻塞操作
        /// </summary>
        bool IsDequeueBlocking { get; }

        /// <summary>
        /// 启动队列提供程序
        /// </summary>
        /// <returns>异步任务</returns>
        Task Start();

        /// <summary>
        /// 停止队列提供程序
        /// </summary>
        /// <returns>异步任务</returns>
        Task Stop();
    }

    /// <summary>
    /// 队列类型枚举
    /// </summary>
    public enum QueueType 
    { 
        /// <summary>工作流队列</summary>
        Workflow = 0, 
        /// <summary>事件队列</summary>
        Event = 1, 
    }
}
