using System;
using System.Collections.Generic;
using UnityEngine;

namespace WithWhat.DesignPattern
{
    public class Mediator:Singleton<Mediator>
    {
        private Mediator() { }

        public delegate void OnEvent(int key, params object[] param);

        /// <summary>
        /// 频段字典，频段中存放消息字典
        /// </summary>
        private readonly Dictionary<Type, Dictionary<int, ListenerWarp>> _frequencyDic = new Dictionary<Type, Dictionary<int, ListenerWarp>>();

        #region 内部结构
        private class ListenerWarp
        {
            private LinkedList<OnEvent> mEventList;

            public bool Fire(int key, params object[] param)
            {
                if (mEventList == null)
                {
                    return false;
                }

                var next = mEventList.First;

                while (next != null)
                {
                    next.Value(key,param);
                    next = next.Next;
                }
                return true;
            }

            public bool Add(OnEvent listener)
            {
                if (mEventList == null)
                {
                    mEventList = new LinkedList<OnEvent>();
                }
                if (mEventList.Contains(listener))
                {
                    return false;
                }
                mEventList.AddLast(listener);
                return true;
            }

            public void Remove(OnEvent listener)
            {
                if (mEventList == null)
                {
                    return;
                }

                mEventList.Remove(listener);
            }

            public void RemoveAll()
            {
                if (mEventList == null)
                {
                    return;
                }
                mEventList.Clear();
            }
        }
        #endregion

        #region 功能函数


        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="T">消息枚举类型</typeparam>
        /// <param name="key">消息枚举</param>
        /// <param name="func">事件</param>
        /// <returns></returns>
        public bool Register<T>(T key, OnEvent func) where T : IConvertible
        {
            var frequencyKey = key.GetType();
            var messageKey = key.ToInt32(null);
            // 查找频段
            Dictionary<int, ListenerWarp> messgeDic;
            if (!_frequencyDic.TryGetValue(frequencyKey, out messgeDic))
            {
                messgeDic = new Dictionary<int, ListenerWarp>();
                _frequencyDic.Add(frequencyKey, messgeDic);
            }
            // 查找消息
            ListenerWarp warp;
            if (!messgeDic.TryGetValue(messageKey, out warp))
            {
                warp = new ListenerWarp();
                messgeDic.Add(messageKey, warp);
            }
            if (warp.Add(func))
            {
                return true;
            }
            else
            {
                Debug.Log($"重复订阅 频段：{key.GetType().Name} 消息：{nameof(key)}");
                return false;
            }
        }

        /// <summary>
        /// 取消注册消息中的单个事件
        /// </summary>
        /// <typeparam name="T">消息枚举类型</typeparam>
        /// <param name="key">消息枚举</param>
        /// <param name="func">事件</param>
        public void UnRegister<T>(T key, OnEvent func) where T : IConvertible
        {
            Dictionary<int, ListenerWarp> _messageDic;
            if (_frequencyDic.TryGetValue(key.GetType(), out _messageDic))
            {
                ListenerWarp warp;
                if (_messageDic.TryGetValue(key.ToInt32(null), out warp))
                {
                    warp.Remove(func);
                }
            }
        }

        /// <summary>
        /// 取消注册整个消息
        /// </summary>
        /// <typeparam name="T">消息枚举类型</typeparam>
        /// <param name="key">消息枚举</param>
        public void UnRegister<T>(T key) where T : IConvertible
        {
            Dictionary<int, ListenerWarp> _messageDic;
            if (_frequencyDic.TryGetValue(key.GetType(), out _messageDic))
            {
                ListenerWarp warp;
                if (_messageDic.TryGetValue(key.ToInt32(null), out warp))
                {
                    warp.RemoveAll();
                    warp = null;
                    _messageDic.Remove(key.ToInt32(null));
                }
            }
        }

        public bool Send<T>(T key, params object[] param) where T : IConvertible
        {
            var frequencyKey = key.GetType();
            var messageKey = key.ToInt32(null);
            Dictionary<int, ListenerWarp> messageDic;
            if (!_frequencyDic.TryGetValue(frequencyKey, out messageDic))
            {
                return false;
            }

            ListenerWarp warp;
            if (!messageDic.TryGetValue(messageKey, out warp))
            {
                return false;
            }

            return warp.Fire(messageKey, param);
        }
        #endregion
    }
}