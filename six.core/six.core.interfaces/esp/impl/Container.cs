using System;
using six.core.interfaces.esp.impl.factories;

namespace six.core.interfaces.esp.impl
{
    class Container : IContainer
    {
        #region interface

        #region Put


        public void Put<T>(T obj, string name = null) where T : class
        {
            var objName = name ?? CreateDefaultObjectTypeName<T>();
            PutObjectFactory(objName, new CachedObjectFactory<T>(obj), CreationStrategy.Singleton);
        }

        public void Put<T>(CreationStrategy strategy) where T : class, new()
        {
            PutObjectFactory(CreateDefaultObjectTypeName<T>(), new NewObjectFactory<T>(), strategy);
        }

        public void Put<T>(IObjectFactory<T> factory, CreationStrategy strategy, string name = null) where T : class
        {
            var typeName = name ?? CreateDefaultObjectTypeName<T>();
            PutObjectFactory(typeName, factory, strategy);
        }

        #endregion

        #region Get

        public T Get<T>() where T : class
        {
            return ResolveObjectWithName<T>(CreateDefaultObjectTypeName<T>());
        }

        public T Get<T>(string name) where T : class
        {
            return ResolveObjectWithName<T>(name);
        }

        #endregion

        #endregion

        #region implementation

        private static string CreateDefaultObjectTypeName<T>()
        {
            return typeof(T).FullName;
        }
        private static string CreateDefaultObjectName<T>(T obj)
        {
            return obj.GetType().FullName;
        }

        private void PutObjectFactory<T>(string objectName, IObjectFactory<T> objectFactory, CreationStrategy strategy) where T : class
        {

            IObjectFactory<T> finalFactory;
            switch (strategy)
            {
                case CreationStrategy.Singleton:
                    // wrap with factory making sure only one instance is created
                    finalFactory = new LazyCachedProxyObjectFactory<T>(objectFactory);
                    break;
                case CreationStrategy.Dynamic:
                    finalFactory = objectFactory;
                    break;
                default:
                    throw new Exception("CreationStrategy unknown");
            }

            var containerEntry = new ContainerEntry
                                      {
                                          Name = objectName,
                                          Type = typeof(T),
                                          CreateObjectFun = () => finalFactory.Create()
                                      };
            StoreContainerEntry(containerEntry);
        }

        private void StoreContainerEntry(ContainerEntry container)
        {
            throw new NotImplementedException();
        }

        private T ResolveObjectWithName<T>(string objName)
        {
            //TODO: convert this below into a list of strategies
            //find object definition entry
            //1. match by type name
            //2. match by concrete type / interface implementing
            //3. match by children that are
        }

        #endregion
    }
}
