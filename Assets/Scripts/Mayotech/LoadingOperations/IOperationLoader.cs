namespace Mayotech.AppLoading
{
    public interface IOperationLoader
    {
        void StartOperation();
        LoadingOperationStatus Status { get; set; }
        void InitOperation();
    }
}