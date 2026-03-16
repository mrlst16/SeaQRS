namespace SeaQRS
{
    public abstract class CommandBase<TRequest> : ICommand<TRequest>
    {
        public abstract Task Run(TRequest request);
        public static implicit operator Func<TRequest, Task>(CommandBase<TRequest> command)
            => (request) => command.Run(request);
    }
}
