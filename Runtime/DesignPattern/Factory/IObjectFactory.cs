using System;

namespace WithWhat.DesignPattern
{
    public interface IObjectFactory
    {
        object AcquireObject(Type type);
        TInstance AcquireObject<TInstance>() where TInstance : class, new();
        void ReleaseObject(object obj);
    }
}