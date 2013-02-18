﻿namespace six.core.interfaces.esp.impl.factories
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
