public class Singleton<T> where T : new()
{
    protected static T instance;
    public static T Instance => instance ??= new T();
}