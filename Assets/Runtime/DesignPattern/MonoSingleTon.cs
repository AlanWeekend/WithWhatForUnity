using UnityEngine;

namespace ZCCUtils.DesignPattern
{
    /// <summary>
    /// 单例模板类
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name + "<Singleton>";
                    instance = obj.AddComponent<T>();
                    DontDestroyOnLoad(obj);
                }

                return instance;
            }
        }
    }
}