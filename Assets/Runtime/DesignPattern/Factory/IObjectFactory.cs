using System;

namespace ZCCUtils.DesignPattern
{
    public interface IObjectFactory
    {
        object AcquireObject(Type type);
        object AcquireObject<TInstance>() where TInstance : class, new();
        void ReleaseObject(object obj);
    }
}