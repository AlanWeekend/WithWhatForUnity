using System;
using System.Collections.Generic;

namespace WithWhat.DesignPattern
{
    public class ServiceLocator
    {
        private static SingleTonObejctFactory _singleTonObejctFactory = new SingleTonObejctFactory();
        private static TransientObjectFactory _transientObjectFactory = new TransientObjectFactory();

        private static readonly Dictionary<Type, Func<object>> Container = new Dictionary<Type, Func<object>>();
        /// <summary>
        /// 对每一次请求，只返回唯一的实例
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TInstance"></typeparam>
        public static void RegisterSingleTon<TInterface, TInstance>() where TInstance : class, new()
        {
            Container.Add(typeof(TInterface), Lazy<TInstance>(FactoryType.SingleTon));
        }

        /// <summary>
        /// 对每一次请求，只返回唯一的实例
        /// </summary>
        /// <typeparam name="TInstance"></typeparam>
        public static void RegisterSingleTon<TInstance>() where TInstance : class, new() 
        {
            Container.Add(typeof(TInstance), Lazy<TInstance>(FactoryType.SingleTon));
        }

        /// <summary>
        /// 对每一次请求，返回不同的实例
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TInstance"></typeparam>
        public static void RegisterTransient<TInterface, TInstance>() where TInstance : class, new()
        {
            Container.Add(typeof(TInterface), Lazy<TInstance>(FactoryType.Transient));
        }

        /// <summary>
        /// 对每一次请求，返回不同的实例
        /// </summary>
        /// <typeparam name="TInstance"></typeparam>
        public static void RegistesrTransient<TInstance>() where TInstance : class, new()
        {
            Container.Add(typeof(TInstance), Lazy<TInstance>(FactoryType.Transient));
        }

        /// <summary>
        /// 清空容器
        /// </summary>
        public static void Clear()
        {
            Container.Clear();
        }

        /// <summary>
        /// 从容器中获取一个实例
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        public static TInterface Resolve<TInterface>() where TInterface : class
        {
            return Resolve(typeof(TInterface)) as TInterface;
        }

        /// <summary>
        /// 从容器中获取一个实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static object Resolve(Type type)
        {
            if (!Container.ContainsKey(type))
            {
                return null;
            }
            return Container[type]();
        }

        private static Func<object> Lazy<TInstance>(FactoryType factoryType) where TInstance : class, new()
        {
            return () =>
            {
                switch (factoryType)
                {
                    case FactoryType.SingleTon:
                        return _singleTonObejctFactory.AcquireObject<TInstance>();
                    default:
                        return _transientObjectFactory.AcquireObject<TInstance>();
                }
            };
        }
    }
}