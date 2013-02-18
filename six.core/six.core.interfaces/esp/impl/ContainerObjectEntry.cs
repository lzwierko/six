using System;

namespace six.core.interfaces.esp.impl
{
    /// <summary>
    /// container for a registered object entry
    /// </summary>
    class ContainerEntry
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public Func<object> CreateObjectFun { get; set; }

        public T CreateObject<T>()
        {
            return (T) CreateObjectFun();
        }
    }
}
