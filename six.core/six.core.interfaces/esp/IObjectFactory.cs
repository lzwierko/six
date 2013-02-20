using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace six.core.interfaces.esp
{

    /// <summary>
    /// ejbs creating interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectFactory<out T> where T : class
    {
        T Create();
    }
}
