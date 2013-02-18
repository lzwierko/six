using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using six.core.interfaces.esp;

namespace six.core.interfaces.esp
{
    /// <summary>
    /// EJBs container interface
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// register object in container with optional name
        /// </summary>
        /// <typeparam name="T">object type</typeparam>
        /// <param name="name">object name</param>
        /// <param name="obj">object</param>
        void Put<T>(T obj, string name = null) where T : class;

        /// <summary>
        /// register a type into container with given strategy
        /// the object of the class are created with new()
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="strategy">creation strategy</param>
        void Put<T>(CreationStrategy strategy) where T : class, new();

        /// <summary>
        /// register a type into container with given object factory, strategy  and optional name
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="factory">factory</param>
        /// <param name="strategy">creation strategy</param>
        /// <param name="name"> </param>
        void Put<T>(IObjectFactory<T> factory, CreationStrategy strategy, string name = null) where T : class;

        /// <summary>
        /// get object of given type from container matching type criteria 
        /// - for class: exactly this class or any inheritating class
        /// - for interfaces: any object inheriatitng the interface
        /// </summary>
        /// <typeparam name="T">type of object to get</typeparam>
        /// <returns>object or null if no object matches type criteria</returns>
        T Get<T>() where T : class;

        /// <summary>
        /// get object of given type from container matching type criteria and the name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        T Get<T>(string name) where T : class;
    }
}
