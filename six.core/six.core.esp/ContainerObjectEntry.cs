using System;

namespace six.core.esp
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
