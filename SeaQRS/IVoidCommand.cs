namespace SeaQRS
{
    public interface IVoidCommand<TRequest>
    {
        Task Run(TRequest request);
    }
}