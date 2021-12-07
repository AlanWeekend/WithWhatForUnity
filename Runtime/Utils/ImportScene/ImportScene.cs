using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using WithWhat.DesignPattern;

namespace WithWhat.Utils.ImportScene
{
    public class ImportScene:Singleton<ImportScene>
    {
        /// <summary>
        /// 自定义加载方法
        /// </summary>
        private Func<string, UnityEngine.Object> Load;
        /// <summary>
        /// 自定义异步加载方法
        /// </summary>
        private Action<List<string>, Action<List<UnityEngine.Object>>> AsyncLoad;
        /// <summary>
        /// 自定义卸载方法，(Prefab路径，Prefab)
        /// </summary>
        private Action<string, GameObject> Unload;
        /// <summary>
        /// 缓存已加实例化的对象 <GameObjectName,GameOject>
        /// </summary>
        private Dictionary<string, GameObject> _gameObjectCache;
        /// <summary>
        /// 缓存实例化的对应引用的预制体 <GameObject,Prefab路径>
        /// </summary>
        private Dictionary<GameObject, string> _gameObjectReference;
        /// <summary>
        /// 缓存已加载过的预制体 <Prefab路径,Prefab>
        /// </summary>
        private Dictionary<string, GameObject> _prefabCache;
        /// <summary>
        /// 预制体引用计数 <Prefab路径，引用数>
        /// </summary>
        private Dictionary<string, int> _prefabReferenceCount;
        /// <summary>
        /// 已加载的场景缓存
        /// </summary>
        private Dictionary<string, GameObject> _sceneCache;

        #region 构造
        private ImportScene() { }
        #endregion

        /// <summary>
        /// 初始化，全局执行一次
        /// </summary>
        /// <param name="load">自定义同步加载方法</param>
        /// <param name="asyncLoad">自定义异步加载方法</param>
        /// <param name="unload">自定义卸载方法</param>
        public void Init(Func<string, UnityEngine.Object> load, Action<List<string>, Action<List<UnityEngine.Object>>> asyncLoad, Action<string, GameObject> unload)
        {
            Load = load;
            AsyncLoad = asyncLoad;
            Unload = unload;
        }

        #region 对外提供的方法
        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="SceneName"></param>
        /// <returns></returns>
        public GameObject LoadScene(string SceneName)
        {
            // 加载配置文件
            var config = LoadConfig(SceneName);
            // 收集配置文件中的Prefab路径
            var prefabPaths = new List<string>();
            GetPrefabPathsInConfig(config, prefabPaths);
            // 加载所需的Prefab
            LoadPrefab(prefabPaths);
            // 实例化Prefab
            return LoadScene(config);
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="SceneName"></param>
        /// <param name="complated"></param>
        public void AsyncLoadScene(string SceneName,Action<GameObject> complated)
        {
            // 加载配置文件
            var config = LoadConfig(SceneName);
            // 收集配置文件中的Prefab路径
            var prefabPaths = new List<string>();
            GetPrefabPathsInConfig(config, prefabPaths);
            // 异步所需的Prefab
            AsyncLoadPrefab(prefabPaths, prefabs =>
            {
                // 实例化Prefab
                complated?.Invoke(LoadScene(config));
            });
        }

        /// <summary>
        /// 卸载场景，包括清除已加载的GameObject缓存和卸载Assets
        /// </summary>
        /// <param name="SceneName"></param>
        public void UnloadScene(string SceneName)
        {
            if (_sceneCache == null) return;
            if (!_sceneCache.ContainsKey(SceneName)) return;
            var sceneGameObject = _sceneCache[SceneName];
            Destory(sceneGameObject);
            _sceneCache.Remove(SceneName);
        }
        #endregion

        #region 内部方法
        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private GameObject LoadScene(ExportSceneConfig config)
        {
            var scene = Generate(config, null);
            if (_sceneCache == null) _sceneCache = new Dictionary<string, GameObject>();
            if (!_sceneCache.ContainsKey(config.GameObjectName)) _sceneCache.Add(config.GameObjectName, scene);
            return scene;
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="SceneName"></param>
        /// <returns></returns>
        private ExportSceneConfig LoadConfig(string SceneName)
        {
            var configTxt = Load(GetConfigPath(SceneName)) as TextAsset;
            if (configTxt == null)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<ExportSceneConfig>(configTxt.text);
        }

        /// <summary>
        /// 根据场景配置文件生成场景内容
        /// </summary>
        /// <param name="exportSceneConfig"></param>
        private GameObject Generate(ExportSceneConfig exportSceneConfig, Transform parent)
        {
            if (exportSceneConfig == null) return null;
            if (!exportSceneConfig.ActiveSelf) return null;
            var go = SpawnGameObject(exportSceneConfig);

            if (go != null)
            {
                go.name = exportSceneConfig.GameObjectName;

                if (parent != null)
                {
                    go.transform.parent = parent;
                }

                // 设置本地坐标
                go.transform.localPosition = Vector3Utils.StringToVector3(exportSceneConfig.Local.Position);
                go.transform.localEulerAngles = Vector3Utils.StringToVector3(exportSceneConfig.Local.Rotation);
                go.transform.localScale = Vector3Utils.StringToVector3(exportSceneConfig.Local.Scale);

                // 添加触发器
                if (exportSceneConfig.Triggers != null && exportSceneConfig.Triggers.Count > 0)
                {
                    var colliders = go.GetComponents<Collider>();
                    if (colliders == null || colliders.Length == 0)
                    {
                        foreach (var trigger in exportSceneConfig.Triggers)
                        {
                            AddTrigger(go, trigger);
                        }
                    }
                }
            }
            else
            {
                Debug.LogErrorFormat("生成失败 {0} {1}", exportSceneConfig.GameObjectName, exportSceneConfig.PrefabPath);
            }

            foreach (var child in exportSceneConfig.Childs)
            {
                Generate(child, go.transform);
            }
            return go;
        }

        /// <summary>
        /// 销毁物体
        /// </summary>
        /// <param name="sceneGameObjet">场景物体</param>
        private void Destory(GameObject sceneGameObjet)
        {
            if (sceneGameObjet == null) return;

            // 先删除子物体，再删除父物体，否则删除父物体的时候子物体也一并销毁了，就无法知道引用了什么Prefab了
            if (sceneGameObjet.transform.childCount != 0)
            {
                for (int i = 0; i < sceneGameObjet.transform.childCount; i++)
                {
                    Destory(sceneGameObjet.transform.GetChild(i).gameObject);
                }
                DestoryGameObject(sceneGameObjet);
            }
            else
            {
                DestoryGameObject(sceneGameObjet);
            }
        }

        /// <summary>
        /// 生成物体
        /// </summary>
        /// <param name="prefabPath"></param>
        /// <returns></returns>
        private GameObject SpawnGameObject(ExportSceneConfig config)
        {
            // 优先从缓存中拿
            if (_gameObjectCache == null || !_gameObjectCache.ContainsKey(config.GameObjectName))
            {
                if (_gameObjectCache == null) _gameObjectCache = new Dictionary<string, GameObject>();
                // 没有prefab路径的，只是一个节点，生成出来对应的空GameObject
                if (string.IsNullOrEmpty(config.PrefabPath))
                {
                    return new GameObject();
                }
                else
                {
                    var prefabPath = Path.GetFileNameWithoutExtension(config.PrefabPath);

                    var prefab = GetPrefab(prefabPath);
                    if (prefab!=null)
                    {
                        var go = GameObject.Instantiate(prefab);
                        _gameObjectCache.Add(config.GameObjectName, go);
                        // 记录GameObject引用的Prefab
                        if (_gameObjectReference == null) _gameObjectReference = new Dictionary<GameObject, string>();
                        if (!_gameObjectReference.ContainsKey(go)) _gameObjectReference.Add(go, prefabPath); 
                    }
                    else
                    {
                        Debug.LogWarningFormat("Prefab加载失败,路径：{0}", config.PrefabPath);
                        return null;
                    }
                }
            }
            return _gameObjectCache[config.GameObjectName];
        }

        /// <summary>
        /// 销毁物体
        /// </summary>
        /// <param name="gameObject"></param>
        private void DestoryGameObject(GameObject gameObject)
        {
            // 普通节点直接删除
            if (gameObject.GetComponent<MeshFilter>() == null)
            {
                GameObject.Destroy(gameObject);
            }
            else
            {
                if (_gameObjectCache != null && _gameObjectCache.ContainsKey(gameObject.name))
                {
                    GameObject prefab = null;
                    string prefabPath = string.Empty;
                    // 引用计数-1
                    if (_gameObjectReference != null && _gameObjectReference.ContainsKey(gameObject))
                    {
                        prefabPath = _gameObjectReference[gameObject];
                        if (_prefabReferenceCount != null && _prefabReferenceCount.ContainsKey(prefabPath))
                        {
                            if (_prefabReferenceCount[prefabPath] > 0)
                            {
                                _prefabReferenceCount[prefabPath]--;
                            }
                            else
                            {
                                if (_prefabCache != null && _prefabCache.ContainsKey(prefabPath))
                                {
                                    prefab = _prefabCache[prefabPath];
                                    _prefabCache.Remove(prefabPath);
                                }
                            }
                        }
                    }
                    // 销毁游戏对象
                    GameObject.Destroy(gameObject);

                    // 卸载掉没有引用的prefab
                    if (prefab != null || !string.IsNullOrEmpty(prefabPath))
                    {
                        Unload?.Invoke(prefabPath, prefab);
                    }
                }
            }
        }

        /// <summary>
        /// 同步加载Prefab
        /// </summary>
        /// <param name="prefabPaths">prefab列表</param>
        private void LoadPrefab(List<string> prefabPaths)
        {
            // 找出所有未加载的Prefab路径
            var unLoadPrefabPaths = GetNotLoadedPrefaPaths(prefabPaths);
            // 加载这些Prefab
            foreach (var unloadPrefabPath in unLoadPrefabPaths)
            {
                var prefab = Load?.Invoke(unloadPrefabPath) as GameObject;
                if(_prefabCache==null) _prefabCache = new Dictionary<string, GameObject>();
                _prefabCache.Add(prefab.name, prefab);
            }
        }

        /// <summary>
        /// 异步加载Prefab
        /// </summary>
        /// <param name="prefabPaths">prefab列表</param>
        /// <param name="complate">加载完成的回调</param>
        private void AsyncLoadPrefab(List<string> prefabPaths, Action<List<UnityEngine.Object>> complate)
        {
            // 找出所有未加载的预制体路径
            var unLoadPrefabPaths = GetNotLoadedPrefaPaths(prefabPaths);
            // 异步加载这些预制体
            AsyncLoad?.Invoke(unLoadPrefabPaths, prefabs =>
            {
                // 加载完成后缓存这些预制体
                foreach (var prefab in prefabs)
                {
                    _prefabCache.Add(prefab.name, prefab as GameObject);
                }

                // 调用加载完成回调
                complate?.Invoke(prefabs);
            });
        }

        /// <summary>
        /// 获取Prefab
        /// </summary>
        /// <param name="prefabPath">预制体名</param>
        /// <returns></returns>
        private GameObject GetPrefab(string prefabPath)
        {
            if (_prefabCache.ContainsKey(prefabPath))
            {
                var prefab = _prefabCache[prefabPath];

                // 添加Prefab引用计数
                if (_prefabReferenceCount == null) _prefabReferenceCount = new Dictionary<string, int>();
                if (!_prefabReferenceCount.ContainsKey(prefabPath)) _prefabReferenceCount.Add(prefabPath, 0);
                _prefabReferenceCount[prefabPath]++;

                return prefab;
            }
            return null;
        }

        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        /// <param name="SceneName"></param>
        /// <returns></returns>
        private string GetConfigPath(string SceneName)
        {
            return Path.Combine("Configs", SceneName);
        }

        /// <summary>
        /// 获取配置文件中的所有Prefab路径
        /// </summary>
        /// <param name="config">配置文件</param>
        /// <param name="prefabPaths">Prefab路径</param>
        private void GetPrefabPathsInConfig(ExportSceneConfig config, List<string> prefabPaths)
        {
            if (config == null) return;
            if (prefabPaths == null) prefabPaths = new List<string>();
            if (!string.IsNullOrEmpty(config.PrefabPath))
            {
                prefabPaths.Add(FileUtils.RemovePathExtision(config.PrefabPath));
            }
            foreach (var child in config.Childs)
            {
                GetPrefabPathsInConfig(child, prefabPaths);
            }
        }

        /// <summary>
        /// 获取未加载的预制体路径列表
        /// </summary>
        /// <param name="prefabPaths">当前需要加载的预制体路径列表</param>
        /// <returns></returns>
        private List<string> GetNotLoadedPrefaPaths(List<string> prefabPaths)
        {
            if (_prefabCache == null) _prefabCache = new Dictionary<string, GameObject>();
            // 去除重复的prefab地址
            prefabPaths = prefabPaths.Distinct().ToList();
            // 找出所有未加载的prefab
            var unloadPrefabPaths = prefabPaths.FindAll(p => !_prefabCache.ContainsKey(p));
            return unloadPrefabPaths;
        }

        /// <summary>
        /// 添加触发器
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private Collider AddTrigger(GameObject go, ExportSceneConfigTrigger sceneConfigTrigger)
        {
            switch (sceneConfigTrigger.TriggerName)
            {
                case nameof(BoxCollider):
                    var boxColliderConfig = JsonConvert.DeserializeObject<ExportSceneConfigTriggerBox>(sceneConfigTrigger.Data);
                    var boxCollider = go.AddComponent<BoxCollider>();
                    boxCollider.center = Vector3Utils.StringToVector3(boxColliderConfig.Center);
                    boxCollider.size = Vector3Utils.StringToVector3(boxColliderConfig.Size);
                    return boxCollider;
                case nameof(CapsuleCollider):
                    var capsuleColliderConfig = JsonConvert.DeserializeObject<ExportSceneConfigTriggerCapsule>(sceneConfigTrigger.Data);
                    var capsuleCollider = go.AddComponent<CapsuleCollider>();
                    capsuleCollider.center = Vector3Utils.StringToVector3(capsuleColliderConfig.Center);
                    capsuleCollider.radius = capsuleColliderConfig.Radius;
                    capsuleCollider.height = capsuleColliderConfig.Height;
                    capsuleCollider.direction = capsuleColliderConfig.Direction;
                    return capsuleCollider;
                case nameof(SphereCollider):
                    var sphereColliderConfig = JsonConvert.DeserializeObject<ExportSceneConfigTriggerSphere>(sceneConfigTrigger.Data);
                    var sphereCollider = go.AddComponent<SphereCollider>();
                    sphereCollider.center = Vector3Utils.StringToVector3(sphereColliderConfig.Center);
                    sphereCollider.radius = sphereColliderConfig.Radius;
                    return sphereCollider;
                default:
                    return null;
            }
        } 
        #endregion
    }
}