using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using six.core.esp.factories;
using six.core.interfaces.esp;

namespace six.core.esp
{
    public class Container : IContainer
    {
        #region fields

        private readonly IDictionary<string, ContainerEntry> _entries = new ConcurrentDictionary<string, ContainerEntry>();
        private readonly IDictionary<Type, ContainerEntry> _typeEntries = new ConcurrentDictionary<Type, ContainerEntry>();
        #endregion

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

        #region Remove

        public void Remove<T>()
        {
            // remove both from _entries and _typeEntries
            _typeEntries.Remove(typeof(T));
            var keysToRemove = _entries.Where(kv => kv.Value.Type == typeof(T)).Select(kv => kv.Key).ToList();
            foreach (var key in keysToRemove)
            {
                _entries.Remove(key);
            }

        }

        public void Remove<T>(string name)
        {
            // remove from _entries
            var value = _entries.FirstOrDefault(kv => kv.Key == name).Value;
            _entries.Remove(name);
            // remove from _typeEntries only if this entry is the default
            var toRemove = _typeEntries.FirstOrDefault(kv => kv.Value == value).Value;
            if (toRemove != null)
            {
                _typeEntries.Remove(typeof(T));
            }
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
            if (GetByName<T>(objectName) != null)
            {
                throw new ArgumentException(string.Format("object with name {1} of type {0} already added", typeof(T).FullName, objectName));
            }

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

        /// <summary>
        /// save the actual mapping
        /// </summary>
        /// <param name="entry"></param>
        private void StoreContainerEntry(ContainerEntry entry)
        {
            // save name
            _entries[entry.Name] = entry;
            // register only first entered type
            if (!_typeEntries.ContainsKey(entry.Type))
            {
                _typeEntries[entry.Type] = entry;
            }
        }


        private T ResolveObjectWithName<T>(string objName) where T : class
        {
            // execute strategies to find the object in given priority
            // by name
            // by exact type
            // by being assignable
            var resolvers = new Func<string, T>[]
                                {
                                    GetByName<T>,
                                    GetByType<T>,
                                    GetByAssignable<T>,
                                };
            return resolvers.Select(r => r(objName)).FirstOrDefault(obj => obj != null);
        }


        /// <summary>
        /// try to matchh object basing on interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objName"></param>
        /// <returns></returns>
        private T GetByAssignable<T>(string objName) where T : class
        {
            var t = typeof(T);
            var func = _typeEntries
                .Where(t1 => t.IsAssignableFrom(t1.Value.Type))
                .Select(t1 => (Func<T>)t1.Value.CreateObject<T>)
                .FirstOrDefault();
            return func != null ? func() : null;

        }       

        /// <summary>
        /// try to match object basing on it's type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objName">unused</param>
        /// <returns></returns>
        private T GetByType<T>(string objName) where T : class
        {
            if (!_typeEntries.ContainsKey(typeof(T)))
                return null;
            var entry = _typeEntries[typeof(T)];
            return entry == null
                ? null
                : entry.CreateObject<T>();
        }

        /// <summary>
        /// resolve obect of given type using name only
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <param name="objName">object name</param>
        /// <returns>instance of objec</returns>
        private T GetByName<T>(string objName) where T : class
        {
            if (!_entries.ContainsKey(objName))
                return null;
            var entry = _entries[objName];
            return entry == null
                ? null
                : entry.CreateObject<T>();
        }

        #endregion
    }
}
