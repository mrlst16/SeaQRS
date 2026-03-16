namespace SeaQRS
{
    public interface ICommand<TRequest>
    {
        Task Run(TRequest request);
    }
}
