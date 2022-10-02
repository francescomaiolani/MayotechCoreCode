namespace Mayotech.UGSConfig
{
    public interface IConfig<T>
    {
        T Data { get; set; }
        string ConfigKey { get; }
    }
}