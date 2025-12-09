using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;
using WorkflowCore.Interface;
using WorkflowCore.Services;

namespace WorkflowCore.Models
{
    public class WorkflowOptions
    {
        internal Func<IServiceProvider, IPersistenceProvider> PersistenceFactory;
        internal Func<IServiceProvider, IQueueProvider> QueueFactory;
        internal Func<IServiceProvider, ILifeCycleEventHub> EventHubFactory;
        internal TimeSpan PollInterval;
        internal TimeSpan IdleTime;
        internal TimeSpan ErrorRetryInterval;
        internal int MaxConcurrentWorkflows = Math.Max(Environment.ProcessorCount, 5);

        public IServiceCollection Services { get; private set; }

        public WorkflowOptions(IServiceCollection services)
        {
            Services = services;
            PollInterval = TimeSpan.FromSeconds(10);
            IdleTime = TimeSpan.FromMilliseconds(100);
            ErrorRetryInterval = TimeSpan.FromSeconds(0.2);

            QueueFactory = new Func<IServiceProvider, IQueueProvider>(sp => new SingleNodeQueueProvider());
            PersistenceFactory = new Func<IServiceProvider, IPersistenceProvider>(sp => sp.GetRequiredService<IPersistenceProvider>());
            EventHubFactory = new Func<IServiceProvider, ILifeCycleEventHub>(sp => new SingleNodeEventHub(sp.GetService<ILoggerFactory>()));
        }

        /// <summary>
        /// 是否启用工作流处理。默认值为 true。
        /// </summary>
        public bool EnableWorkflows { get; set; } = true;

        /// <summary>
        /// 是否启用事件处理。默认值为 true。
        /// </summary>
        public bool EnableEvents { get; set; } = true;

        public void UsePersistence(Func<IServiceProvider, IPersistenceProvider> factory)
        {
            PersistenceFactory = factory;
        }

        public void UseQueueProvider(Func<IServiceProvider, IQueueProvider> factory)
        {
            QueueFactory = factory;
        }

        public void UseEventHub(Func<IServiceProvider, ILifeCycleEventHub> factory)
        {
            EventHubFactory = factory;
        }

        public void UsePollInterval(TimeSpan interval)
        {
            PollInterval = interval;
        }

        public void UseErrorRetryInterval(TimeSpan interval)
        {
            ErrorRetryInterval = interval;
        }

        public void UseIdleTime(TimeSpan interval)
        {
            IdleTime = interval;
        }

        public void UseMaxConcurrentWorkflows(int maxConcurrentWorkflows)
        {
            MaxConcurrentWorkflows = maxConcurrentWorkflows;
        }
    }
        
}
