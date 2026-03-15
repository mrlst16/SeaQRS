namespace SeaQRS
{
    public abstract class CommandBase<TResponse> : ICommand<TResponse>
    {
        public abstract Task<TResponse> Run();
        public static implicit operator Func<Task<TResponse>>(CommandBase<TResponse> command)
            => () => command.Run();
    }
}