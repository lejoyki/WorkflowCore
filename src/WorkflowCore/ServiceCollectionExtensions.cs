using System;
using System.Linq;
using WorkflowCore.Interface;
using WorkflowCore.Services;
using WorkflowCore.Models;
using Microsoft.Extensions.ObjectPool;
using WorkflowCore.Primitives;
using WorkflowCore.Services.BackgroundTasks;
using WorkflowCore.Services.ErrorHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWorkflow(this IServiceCollection services, Action<WorkflowOptions> setupAction = null)
        {
            if (services.Any(x => x.ServiceType == typeof(WorkflowOptions)))
                throw new InvalidOperationException("Workflow services already registered");

            var options = new WorkflowOptions(services);
            setupAction?.Invoke(options);
            services.AddSingleton<IPersistenceProvider, MemoryPersistenceProvider>();
            services.AddSingleton<IWorkflowRepository>(options.PersistenceFactory);
            services.AddSingleton<ISubscriptionRepository>(options.PersistenceFactory);
            services.AddSingleton<IEventRepository>(options.PersistenceFactory);

            services.AddSingleton<IQueueProvider>(options.QueueFactory);
            services.AddSingleton<ILifeCycleEventHub>(options.EventHubFactory);

            services.AddSingleton<IWorkflowRegistry, WorkflowRegistry>();
            services.AddSingleton<WorkflowOptions>(options);

            if (options.EnableWorkflows)
            {
                services.AddTransient<IBackgroundTask, WorkflowConsumer>();
            }

            if (options.EnableEvents)
            {
                services.AddTransient<IBackgroundTask, EventConsumer>();
            }

            services.AddTransient<IWorkflowErrorHandler, RetryHandler>();
            services.AddTransient<IWorkflowErrorHandler, TerminateHandler>();
            services.AddTransient<IWorkflowErrorHandler, SuspendHandler>();

            services.AddSingleton<IWorkflowController, WorkflowController>();
            services.AddSingleton<IWorkflowHost, WorkflowHost>();
            services.AddTransient<IStepExecutor, StepExecutor>();
            services.AddTransient<IWorkflowMiddlewareRunner, WorkflowMiddlewareRunner>();
            services.AddTransient<IScopeProvider, ScopeProvider>();
            services.AddTransient<IWorkflowExecutor, WorkflowExecutor>();
            services.AddTransient<ICancellationProcessor, CancellationProcessor>();
            services.AddTransient<IWorkflowBuilder, WorkflowBuilder>();
            services.AddTransient<IExecutionResultProcessor, ExecutionResultProcessor>();
            services.AddTransient<IExecutionPointerFactory, ExecutionPointerFactory>();

            services.AddTransient<IPooledObjectPolicy<IPersistenceProvider>, InjectedObjectPoolPolicy<IPersistenceProvider>>();
            services.AddTransient<IPooledObjectPolicy<IWorkflowExecutor>, InjectedObjectPoolPolicy<IWorkflowExecutor>>();

            return services;
        }

        /// <summary>
        /// Adds a middleware that will run around the execution of a workflow step.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <param name="factory">Optionally configure using your own factory.</param>
        /// <typeparam name="TMiddleware">The type of middleware.
        /// It must implement <see cref="IWorkflowStepMiddleware"/>.</typeparam>
        /// <returns>The services collection for chaining.</returns>
        public static IServiceCollection AddWorkflowStepMiddleware<TMiddleware>(
            this IServiceCollection services,
            Func<IServiceProvider, TMiddleware> factory = null)
            where TMiddleware : class, IWorkflowStepMiddleware =>
                factory == null
                    ? services.AddTransient<IWorkflowStepMiddleware, TMiddleware>()
                    : services.AddTransient<IWorkflowStepMiddleware, TMiddleware>(factory);

        /// <summary>
        /// Adds a middleware that will run either before a workflow is kicked off or after
        /// a workflow completes. Specify the phase of the workflow execution process that
        /// you want to execute this middleware using <see cref="IWorkflowMiddleware.Phase"/>.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <param name="factory">Optionally configure using your own factory.</param>
        /// <typeparam name="TMiddleware">The type of middleware.
        /// It must implement <see cref="IWorkflowMiddleware"/>.</typeparam>
        /// <returns>The services collection for chaining.</returns>
        public static IServiceCollection AddWorkflowMiddleware<TMiddleware>(
            this IServiceCollection services,
            Func<IServiceProvider, TMiddleware> factory = null)
            where TMiddleware : class, IWorkflowMiddleware =>
                factory == null
                    ? services.AddTransient<IWorkflowMiddleware, TMiddleware>()
                    : services.AddTransient<IWorkflowMiddleware, TMiddleware>(factory);
    }
}

