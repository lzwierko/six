using six.core.interfaces.esp;

namespace six.core.esp.factories
{

    /// <summary>
    /// object factory returns an object reated with new()
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class NewObjectFactory<T> : IObjectFactory<T> where T : class, new()
    {
        public T Create()
        {
            return new T();
        }
    }
}
