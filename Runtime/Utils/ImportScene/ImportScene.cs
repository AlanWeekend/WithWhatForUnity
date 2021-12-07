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
        /// �Զ�����ط���
        /// </summary>
        private Func<string, UnityEngine.Object> Load;
        /// <summary>
        /// �Զ����첽���ط���
        /// </summary>
        private Action<List<string>, Action<List<UnityEngine.Object>>> AsyncLoad;
        /// <summary>
        /// �Զ���ж�ط�����(Prefab·����Prefab)
        /// </summary>
        private Action<string, GameObject> Unload;
        /// <summary>
        /// �����Ѽ�ʵ�����Ķ��� <GameObjectName,GameOject>
        /// </summary>
        private Dictionary<string, GameObject> _gameObjectCache;
        /// <summary>
        /// ����ʵ�����Ķ�Ӧ���õ�Ԥ���� <GameObject,Prefab·��>
        /// </summary>
        private Dictionary<GameObject, string> _gameObjectReference;
        /// <summary>
        /// �����Ѽ��ع���Ԥ���� <Prefab·��,Prefab>
        /// </summary>
        private Dictionary<string, GameObject> _prefabCache;
        /// <summary>
        /// Ԥ�������ü��� <Prefab·����������>
        /// </summary>
        private Dictionary<string, int> _prefabReferenceCount;
        /// <summary>
        /// �Ѽ��صĳ�������
        /// </summary>
        private Dictionary<string, GameObject> _sceneCache;

        #region ����
        private ImportScene() { }
        #endregion

        /// <summary>
        /// ��ʼ����ȫ��ִ��һ��
        /// </summary>
        /// <param name="load">�Զ���ͬ�����ط���</param>
        /// <param name="asyncLoad">�Զ����첽���ط���</param>
        /// <param name="unload">�Զ���ж�ط���</param>
        public void Init(Func<string, UnityEngine.Object> load, Action<List<string>, Action<List<UnityEngine.Object>>> asyncLoad, Action<string, GameObject> unload)
        {
            Load = load;
            AsyncLoad = asyncLoad;
            Unload = unload;
        }

        #region �����ṩ�ķ���
        /// <summary>
        /// ���س���
        /// </summary>
        /// <param name="SceneName"></param>
        /// <returns></returns>
        public GameObject LoadScene(string SceneName)
        {
            // ���������ļ�
            var config = LoadConfig(SceneName);
            // �ռ������ļ��е�Prefab·��
            var prefabPaths = new List<string>();
            GetPrefabPathsInConfig(config, prefabPaths);
            // ���������Prefab
            LoadPrefab(prefabPaths);
            // ʵ����Prefab
            return LoadScene(config);
        }

        /// <summary>
        /// �첽���س���
        /// </summary>
        /// <param name="SceneName"></param>
        /// <param name="complated"></param>
        public void AsyncLoadScene(string SceneName,Action<GameObject> complated)
        {
            // ���������ļ�
            var config = LoadConfig(SceneName);
            // �ռ������ļ��е�Prefab·��
            var prefabPaths = new List<string>();
            GetPrefabPathsInConfig(config, prefabPaths);
            // �첽�����Prefab
            AsyncLoadPrefab(prefabPaths, prefabs =>
            {
                // ʵ����Prefab
                complated?.Invoke(LoadScene(config));
            });
        }

        /// <summary>
        /// ж�س�������������Ѽ��ص�GameObject�����ж��Assets
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

        #region �ڲ�����
        /// <summary>
        /// ���س���
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
        /// ���������ļ�
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
        /// ���ݳ��������ļ����ɳ�������
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

                // ���ñ�������
                go.transform.localPosition = Vector3Utils.StringToVector3(exportSceneConfig.Local.Position);
                go.transform.localEulerAngles = Vector3Utils.StringToVector3(exportSceneConfig.Local.Rotation);
                go.transform.localScale = Vector3Utils.StringToVector3(exportSceneConfig.Local.Scale);

                // ��Ӵ�����
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
                Debug.LogErrorFormat("����ʧ�� {0} {1}", exportSceneConfig.GameObjectName, exportSceneConfig.PrefabPath);
            }

            foreach (var child in exportSceneConfig.Childs)
            {
                Generate(child, go.transform);
            }
            return go;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="sceneGameObjet">��������</param>
        private void Destory(GameObject sceneGameObjet)
        {
            if (sceneGameObjet == null) return;

            // ��ɾ�������壬��ɾ�������壬����ɾ���������ʱ��������Ҳһ�������ˣ����޷�֪��������ʲôPrefab��
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
        /// ��������
        /// </summary>
        /// <param name="prefabPath"></param>
        /// <returns></returns>
        private GameObject SpawnGameObject(ExportSceneConfig config)
        {
            // ���ȴӻ�������
            if (_gameObjectCache == null || !_gameObjectCache.ContainsKey(config.GameObjectName))
            {
                if (_gameObjectCache == null) _gameObjectCache = new Dictionary<string, GameObject>();
                // û��prefab·���ģ�ֻ��һ���ڵ㣬���ɳ�����Ӧ�Ŀ�GameObject
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
                        // ��¼GameObject���õ�Prefab
                        if (_gameObjectReference == null) _gameObjectReference = new Dictionary<GameObject, string>();
                        if (!_gameObjectReference.ContainsKey(go)) _gameObjectReference.Add(go, prefabPath); 
                    }
                    else
                    {
                        Debug.LogWarningFormat("Prefab����ʧ��,·����{0}", config.PrefabPath);
                        return null;
                    }
                }
            }
            return _gameObjectCache[config.GameObjectName];
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="gameObject"></param>
        private void DestoryGameObject(GameObject gameObject)
        {
            // ��ͨ�ڵ�ֱ��ɾ��
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
                    // ���ü���-1
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
                    // ������Ϸ����
                    GameObject.Destroy(gameObject);

                    // ж�ص�û�����õ�prefab
                    if (prefab != null || !string.IsNullOrEmpty(prefabPath))
                    {
                        Unload?.Invoke(prefabPath, prefab);
                    }
                }
            }
        }

        /// <summary>
        /// ͬ������Prefab
        /// </summary>
        /// <param name="prefabPaths">prefab�б�</param>
        private void LoadPrefab(List<string> prefabPaths)
        {
            // �ҳ�����δ���ص�Prefab·��
            var unLoadPrefabPaths = GetNotLoadedPrefaPaths(prefabPaths);
            // ������ЩPrefab
            foreach (var unloadPrefabPath in unLoadPrefabPaths)
            {
                var prefab = Load?.Invoke(unloadPrefabPath) as GameObject;
                if(_prefabCache==null) _prefabCache = new Dictionary<string, GameObject>();
                _prefabCache.Add(prefab.name, prefab);
            }
        }

        /// <summary>
        /// �첽����Prefab
        /// </summary>
        /// <param name="prefabPaths">prefab�б�</param>
        /// <param name="complate">������ɵĻص�</param>
        private void AsyncLoadPrefab(List<string> prefabPaths, Action<List<UnityEngine.Object>> complate)
        {
            // �ҳ�����δ���ص�Ԥ����·��
            var unLoadPrefabPaths = GetNotLoadedPrefaPaths(prefabPaths);
            // �첽������ЩԤ����
            AsyncLoad?.Invoke(unLoadPrefabPaths, prefabs =>
            {
                // ������ɺ󻺴���ЩԤ����
                foreach (var prefab in prefabs)
                {
                    _prefabCache.Add(prefab.name, prefab as GameObject);
                }

                // ���ü�����ɻص�
                complate?.Invoke(prefabs);
            });
        }

        /// <summary>
        /// ��ȡPrefab
        /// </summary>
        /// <param name="prefabPath">Ԥ������</param>
        /// <returns></returns>
        private GameObject GetPrefab(string prefabPath)
        {
            if (_prefabCache.ContainsKey(prefabPath))
            {
                var prefab = _prefabCache[prefabPath];

                // ���Prefab���ü���
                if (_prefabReferenceCount == null) _prefabReferenceCount = new Dictionary<string, int>();
                if (!_prefabReferenceCount.ContainsKey(prefabPath)) _prefabReferenceCount.Add(prefabPath, 0);
                _prefabReferenceCount[prefabPath]++;

                return prefab;
            }
            return null;
        }

        /// <summary>
        /// ��ȡ�����ļ�·��
        /// </summary>
        /// <param name="SceneName"></param>
        /// <returns></returns>
        private string GetConfigPath(string SceneName)
        {
            return Path.Combine("Configs", SceneName);
        }

        /// <summary>
        /// ��ȡ�����ļ��е�����Prefab·��
        /// </summary>
        /// <param name="config">�����ļ�</param>
        /// <param name="prefabPaths">Prefab·��</param>
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
        /// ��ȡδ���ص�Ԥ����·���б�
        /// </summary>
        /// <param name="prefabPaths">��ǰ��Ҫ���ص�Ԥ����·���б�</param>
        /// <returns></returns>
        private List<string> GetNotLoadedPrefaPaths(List<string> prefabPaths)
        {
            if (_prefabCache == null) _prefabCache = new Dictionary<string, GameObject>();
            // ȥ���ظ���prefab��ַ
            prefabPaths = prefabPaths.Distinct().ToList();
            // �ҳ�����δ���ص�prefab
            var unloadPrefabPaths = prefabPaths.FindAll(p => !_prefabCache.ContainsKey(p));
            return unloadPrefabPaths;
        }

        /// <summary>
        /// ��Ӵ�����
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