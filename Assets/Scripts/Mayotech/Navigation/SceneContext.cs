namespace Mayotech.Navigation
{
    public interface ISceneContext { }

    
    public class SceneContext<T>
    {
        public T Data { get; private set; }

        public SceneContext(T data)
        {
            Data = data;
        }
    }
}