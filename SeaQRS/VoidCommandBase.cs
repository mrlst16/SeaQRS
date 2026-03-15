namespace SeaQRS
{
    public abstract class VoidCommandBase<TRequest> : IVoidCommand<TRequest>
    {
        public abstract Task Run(TRequest request);
        public static implicit operator Func<TRequest, Task>(VoidCommandBase<TRequest> command)
            => (request) => command.Run(request);
    }
}