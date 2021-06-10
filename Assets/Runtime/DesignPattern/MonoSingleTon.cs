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
                    instance = FindObjectOfType<T>();
                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        Debug.LogWarning($"More than 1: {typeof(T).Name}");
                        return instance;
                    }
                    if (instance == null)
                    {
                        var instanceName = typeof(T).Name;
                        Debug.LogFormat("Instance Name: {0}", instanceName);
                        var instanceObj = GameObject.Find(instanceName);
                        if (!instanceObj)
                            instanceObj = new GameObject(instanceName);
                        instance = instanceObj.AddComponent<T>();
                        DontDestroyOnLoad(instanceObj); //保证实例例不不会被释放

                        Debug.LogFormat("Add New Singleton {0} in Game!",
                        instanceName);
                    }
                    else
                    {
                        Debug.LogFormat("Already exist: {0}", instance.name);
                    }
                }
                return instance;
            }
        }

        private void OnDestroy()
        {
            instance = null;
        }
    }
}