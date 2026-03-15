namespace SeaQRS
{
    public interface ICommand<TResponse>
    {
        Task<TResponse> Run();
    }
}