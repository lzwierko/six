using six.core.interfaces.esp;

namespace six.core.esp.factories
{
    /// <summary>
    /// this factory always returns cached object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class CachedObjectFactory<T> : IObjectFactory<T> where T : class
    {
        private readonly T _obj;

        public CachedObjectFactory(T obj)
        {
            _obj = obj;
        }

        public T Create()
        {
            return _obj;
        }
    }
}
