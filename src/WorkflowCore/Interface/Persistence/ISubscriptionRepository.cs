using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkflowCore.Models;

namespace WorkflowCore.Interface
{
    /// <summary>
    /// 事件订阅存储库接口，提供事件订阅的持久化操作
    /// </summary>
    public interface ISubscriptionRepository
    {        
        /// <summary>
        /// 创建事件订阅
        /// </summary>
        /// <param name="subscription">事件订阅</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>订阅ID</returns>
        Task<string> CreateEventSubscription(EventSubscription subscription, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据条件获取事件订阅列表
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="eventKey">事件键</param>
        /// <param name="asOf">时间点</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>事件订阅集合</returns>
        Task<IEnumerable<EventSubscription>> GetSubscriptions(string eventName, string eventKey, DateTime asOf, CancellationToken cancellationToken = default);

        /// <summary>
        /// 终止事件订阅
        /// </summary>
        /// <param name="eventSubscriptionId">事件订阅ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>异步任务</returns>
        Task TerminateSubscription(string eventSubscriptionId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据ID获取事件订阅
        /// </summary>
        /// <param name="eventSubscriptionId">事件订阅ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>事件订阅</returns>
        Task<EventSubscription> GetSubscription(string eventSubscriptionId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取第一个开放的事件订阅
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="eventKey">事件键</param>
        /// <param name="asOf">时间点</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>事件订阅</returns>
        Task<EventSubscription> GetFirstOpenSubscription(string eventName, string eventKey, DateTime asOf, CancellationToken cancellationToken = default);
    }
}
