namespace SeaQRS
{
    public interface IQuery<TRequest, TResponse>
    {
        Task<TResponse> Run(TRequest request);
    }

    public interface IQuery<TResponse>
    {
        Task<TResponse> Run();
    }
}
