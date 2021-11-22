using System;

namespace WithWhat.DesignPattern
{
    public interface IObjectFactory
    {
        object AcquireObject(Type type);
        object AcquireObject<TInstance>() where TInstance : class, new();
        void ReleaseObject(object obj);
    }
}