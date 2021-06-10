using UnityEngine;

namespace ZCCUtils.DesignPattern
{
    /// <summary>
    /// 单例模板类
    /// </summary>
    public class MonoSingleTon<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    // 如果不存在实例, 则查找所有这个类型的对象
                    instance = FindObjectOfType(typeof(T)) as T;
                    if (instance == null)
                    {
                        // 如果没有找到， 则新建一个
                        GameObject obj = new GameObject(typeof(T).Name);
                        // 强制转换为 T 
                        instance = obj.AddComponent(typeof(T)) as T;
                    }
                }
                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
                instance = this as T;
            else
            {
                GameObject.Destroy(instance);
                instance = this as T;
            }
        }

        private void OnDestroy()
        {
            instance = null;
        }
    }
}