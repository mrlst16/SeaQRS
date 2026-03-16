using Microsoft.Extensions.DependencyInjection;

namespace SeaQRS
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCommand<TCommand, TResponse>(
            this IServiceCollection services,
            ServiceLifetime lifetime
        )
            where TCommand : CommandBase<TResponse>
        {
            services.Add(new ServiceDescriptor(typeof(Func<Task<TResponse>>), typeof(TCommand), lifetime));
            return services;
        }

        public static IServiceCollection AddCommandTransient<TCommand, TResponse>(
            this IServiceCollection services
        )
            where TCommand : CommandBase<TResponse>
        {
            return services.AddCommand<TCommand, TResponse>(ServiceLifetime.Transient);
        }

        public static IServiceCollection AddCommandScoped<TCommand, TResponse>(
            this IServiceCollection services
        )
            where TCommand : CommandBase<TResponse>
        {
            return services.AddCommand<TCommand, TResponse>(ServiceLifetime.Scoped);
        }

        public static IServiceCollection AddCommandSingleton<TCommand, TResponse>(
            this IServiceCollection services
        )
            where TCommand : CommandBase<TResponse>
        {
            return services.AddCommand<TCommand, TResponse>(ServiceLifetime.Singleton);
        }

        public static IServiceCollection AddVoidCommand<TCommand, TRequest>(
            this IServiceCollection services,
            ServiceLifetime lifetime
        )
            where TCommand : VoidCommandBase<TRequest>
        {
            services.Add(new ServiceDescriptor(typeof(Func<TRequest, Task>), typeof(TCommand), lifetime));
            return services;
        }

        public static IServiceCollection AddVoidCommandTransient<TCommand, TRequest>(
            this IServiceCollection services
        )
            where TCommand : VoidCommandBase<TRequest>
        {
            return services.AddVoidCommand<TCommand, TRequest>(ServiceLifetime.Transient);
        }

        public static IServiceCollection AddVoidCommandScoped<TCommand, TRequest>(
            this IServiceCollection services
        )
            where TCommand : VoidCommandBase<TRequest>
        {
            return services.AddVoidCommand<TCommand, TRequest>(ServiceLifetime.Scoped);
        }

        public static IServiceCollection AddVoidCommandSingleton<TCommand, TRequest>(
            this IServiceCollection services
        )
            where TCommand : VoidCommandBase<TRequest>
        {
            return services.AddVoidCommand<TCommand, TRequest>(ServiceLifetime.Singleton);
        }

        public static IServiceCollection AddQuery<TQuery, TRequest, TResponse>(
            this IServiceCollection services,
            ServiceLifetime lifetime
        )
            where TQuery : QueryBase<TRequest, TResponse>
        {
            services.Add(new ServiceDescriptor(typeof(Func<TRequest, Task<TResponse>>), typeof(TQuery), lifetime));
            return services;
        }

        public static IServiceCollection AddQueryTransient<TQuery, TRequest, TResponse>(
            this IServiceCollection services
        )
            where TQuery : QueryBase<TRequest, TResponse>
        {
            return services.AddQuery<TQuery, TRequest, TResponse>(ServiceLifetime.Transient);
        }

        public static IServiceCollection AddQueryScoped<TQuery, TRequest, TResponse>(
            this IServiceCollection services
        )
            where TQuery : QueryBase<TRequest, TResponse>
        {
            return services.AddQuery<TQuery, TRequest, TResponse>(ServiceLifetime.Scoped);
        }

        public static IServiceCollection AddQuerySingleton<TQuery, TRequest, TResponse>(
            this IServiceCollection services
        )
            where TQuery : QueryBase<TRequest, TResponse>
        {
            return services.AddQuery<TQuery, TRequest, TResponse>(ServiceLifetime.Singleton);
        }

        public static IServiceCollection AddQuery<TQuery, TResponse>(
            this IServiceCollection services,
            ServiceLifetime lifetime
        )
            where TQuery : QueryBase<TResponse>
        {
            services.Add(new ServiceDescriptor(typeof(Func<Task<TResponse>>), typeof(TQuery), lifetime));
            return services;
        }

        public static IServiceCollection AddQueryTransient<TQuery, TResponse>(
            this IServiceCollection services
        )
            where TQuery : QueryBase<TResponse>
        {
            return services.AddQuery<TQuery, TResponse>(ServiceLifetime.Transient);
        }

        public static IServiceCollection AddQueryScoped<TQuery, TResponse>(
            this IServiceCollection services
        )
            where TQuery : QueryBase<TResponse>
        {
            return services.AddQuery<TQuery, TResponse>(ServiceLifetime.Scoped);
        }

        public static IServiceCollection AddQuerySingleton<TQuery, TResponse>(
            this IServiceCollection services
        )
            where TQuery : QueryBase<TResponse>
        {
            return services.AddQuery<TQuery, TResponse>(ServiceLifetime.Singleton);
        }
    }
}
