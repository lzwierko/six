using System;

namespace six.core.interfaces.esp.impl.factories
{
    /// <summary>
    /// this factory lazely initializes object and caches and object created by base facotry
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class LazyCachedProxyObjectFactory<T> : IObjectFactory<T> where T : class
    {
        private readonly Lazy<T> _obj;

        public LazyCachedProxyObjectFactory(IObjectFactory<T> objectFactory)
        {
            _obj = new Lazy<T>(objectFactory.Create);
        }

        public T Create()
        {
            return _obj.Value;
        }
    }
}
