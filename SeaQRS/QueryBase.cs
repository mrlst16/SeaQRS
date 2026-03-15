namespace SeaQRS
{
    public abstract class QueryBase<TRequest, TResponse> : IQuery<TRequest, TResponse>
    {
        public abstract Task<TResponse> Run(TRequest request);

        public static implicit operator Func<TRequest, Task<TResponse>>(QueryBase<TRequest, TResponse> command)
            => (request) => command.Run(request);
    }

    public abstract class QueryBase<TResponse> : IQuery<TResponse>
    {
        public abstract Task<TResponse> Run();

        public static implicit operator Func<Task<TResponse>>(QueryBase<TResponse> command)
            => () => command.Run();
    }
}