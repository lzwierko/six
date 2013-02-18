namespace six.core.interfaces.esp.impl.factories
{
    /// <summary>
    /// this factory just proxies the call to another factory
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class ProxyObjectFactory<T> : IObjectFactory<T> where T : class
    {
        private readonly IObjectFactory<T> _objectFactory;

        public ProxyObjectFactory(IObjectFactory<T> objectFactory)
        {
            _objectFactory = objectFactory;
        }

        public T Create()
        {
            return _objectFactory.Create();
        }
    }
}
