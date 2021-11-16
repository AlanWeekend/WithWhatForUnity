using System;
using System.Collections.Generic;

namespace ZCCUtils.DesignPattern
{
    public class PoolObjectFactory : IObjectFactory
    {
        private class PoolData
        { 
            public bool InUse { get; set; }
            public object Obj { get; set; }
        }

        private readonly List<PoolData> _pool;
        private readonly int _max;
        /// <summary>
        /// 如果超过了容器大小，是否限制
        /// </summary>
        private readonly bool _limit;

        public PoolObjectFactory(int max, bool limit)
        {
            _max = max;
            _limit = limit;
            _pool = new List<PoolData>();
        }

        private PoolData GetPoolData(object obj)
        {
            lock (_pool)
            {
                for (int i = 0; i < _pool.Count; i++)
                {
                    var p = _pool[i];
                    if (p.Obj == obj)
                    {
                        return p;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取对象池中真正的对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private object GetObject(Type type)
        {
            lock (_pool)
            {
                if (_pool.Count > 0)
                {
                    if (_pool[0].Obj.GetType() != type)
                    {
                        throw new Exception(string.Format("the Pool Factory only for Type :{0}", _pool[0].Obj.GetType().Name));
                    }
                }

                for (int i = 0; i < _pool.Count; i++)
                {
                    var p = _pool[i];
                    if (!p.InUse)
                    {
                        return p.Obj;
                    }
                }

                if (_pool.Count >= _max && _limit)
                {
                    throw new Exception("max limit is arrived.");
                }

                object obj = Activator.CreateInstance(type, false);
                var p1 = new PoolData
                {
                    InUse = true,
                    Obj = obj
                };

                _pool.Add(p1);
                return obj;
            }
        }

        private void PutObject(object obj)
        {
            var p = GetPoolData(obj);
            if (p != null)
            {
                p.InUse = false;
            }
        }

        public object AcquireObject(Type type)
        {
            return GetObject(type);
        }

        public object AcquireObject<TInstance>() where TInstance : class, new()
        {
            return AcquireObject(typeof(TInstance));
        }

        public void ReleaseObject(object obj)
        {
            if (_pool.Count > _max)
            {
                if (obj is IDisposable)
                {
                    ((IDisposable)obj).Dispose();
                }
                var p = GetPoolData(obj);
                lock (_pool)
                {
                    _pool.Remove(p);
                }
                return;
            }
            PutObject(obj);
        }
    }
}