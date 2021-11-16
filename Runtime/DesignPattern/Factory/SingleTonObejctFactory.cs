using System;
using System.Collections.Generic;

namespace ZCCUtils.DesignPattern
{
    public class SingleTonObejctFactory : IObjectFactory
    {
        private static Dictionary<Type, object> _cacheObjects = null;
        private static readonly object _lock = new object();

        public static Dictionary<Type, object> CacheObjects
        {
            get
            {
                lock (_lock)
                {
                    if (_cacheObjects == null)
                    {
                        _cacheObjects = new Dictionary<Type, object>();
                    }
                    return _cacheObjects;
                }
            }
        }

        public object AcquireObject(Type type)
        {
            if (CacheObjects.ContainsKey(type))
            {
                return CacheObjects[type];
            }
            lock (_lock)
            {
                CacheObjects.Add(type, Activator.CreateInstance(type, false));
                return CacheObjects[type];
            }
        }

        public object AcquireObject<TInstance>() where TInstance : class, new()
        {
            var type = typeof(TInstance);
            if (CacheObjects.ContainsKey(type))
            {
                return CacheObjects[type];
            }
            lock (_lock)
            {
                var instance = new TInstance();
                CacheObjects.Add(type, instance);
                return CacheObjects[type];
            }
        }

        public void ReleaseObject(object obj)
        {
        }
    }
}