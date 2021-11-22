using System;

namespace WithWhat.DesignPattern
{
    public class TransientObjectFactory : IObjectFactory
    {
        public object AcquireObject(Type type)
        {
            return Activator.CreateInstance(type, false);
        }

        public object AcquireObject<TInstance>() where TInstance : class, new()
        {
            return new TInstance();
        }

        public void ReleaseObject(object obj)
        {
        }
    }
}