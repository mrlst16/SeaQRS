namespace SeaQRS
{
    public interface ICommand<TRequest>
    {
        Task Run(TRequest request);
    }

    public interface ICommand<TRequest, TResponse>
    {
        Task<TResponse> Run(TRequest request);
    }
}
