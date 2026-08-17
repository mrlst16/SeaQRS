namespace SeaQRS
{
    public abstract class CommandBase<TRequest> : ICommand<TRequest>
    {
        public abstract Task Run(TRequest request);
        public static implicit operator Func<TRequest, Task>(CommandBase<TRequest> command)
            => (request) => command.Run(request);
    }

    public abstract class CommandBase<TRequest, TResponse> : ICommand<TRequest, TResponse>
    {
        public abstract Task<TResponse> Run(TRequest request);

        public static implicit operator Func<TRequest, Task<TResponse>>(CommandBase<TRequest, TResponse> command)
            => (request) => command.Run(request);
    }
}
