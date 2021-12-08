using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using WithWhat.Utils.ImportScene;

namespace WithWhat.Editor
{
    public class EditorWindowExportScene : EditorWindow
    {

        private GameObject _targetGo;
        /// <summary>
        /// 选择的路径
        /// </summary>
        private string _filePath;
        /// <summary>
        /// 存放Prefab的路径
        /// </summary>
        private string _prefabPath;
        /// <summary>
        /// 存放配置文件的路径
        /// </summary>
        private string _configPath;
        /// <summary>
        /// 存放收集配置的路径
        /// </summary>
        private string _configColletPath;
        /// <summary>
        /// 收集所有的配置文件
        /// </summary>
        private ExportSceneConfigCollect _configCollect;

        /// <summary>
        /// 是否将Prefab存储为相对路径
        /// </summary>
        private bool _isRelative = true;

        public void DrawGUI()
        {
            WithWhatEditorWindowUtil.DrawTitle("场景导出");
            WithWhatEditorWindowUtil.DrawOneContent("说明", @"生成场景配置文件及prefab

使用时的注意事项:
1. 限于Unity本身的机制，Prefab不能生成到StreamingAsset文件下,否则无法加载到依赖关系,即使实例化到场景里,也仅是一个有预制体名字的空物体
2. 不支持变种Prefab，除对Transform组件外的修改都要重新保存一份 Original Prefab
3. 导出前先执行清理冗余操作！！！

目录结构说明：
1. 'ConfigCollet.json' ：所有配置文件的引用
2. 'Configs' : 存放所有的配置文件
3. 'Prefabs' : 存放所有的预制体文件
");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"保存路径:{_filePath}");
            if (GUILayout.Button("选择路径"))
            {
                // 保存选择的路径
                _filePath = EditorUtility.SaveFolderPanel("选择路径", Application.dataPath, "");

                // 保存收集配置的路径
                _configColletPath = $"{_filePath}/ConfigCollet.json";
                // 读取配置
                if (File.Exists(_configColletPath))
                {
                    var colletTxt = File.ReadAllText(_configColletPath);
                    _configCollect = JsonConvert.DeserializeObject<ExportSceneConfigCollect>(colletTxt);
                }

                // 保存Config路径
                _configPath = $"{_filePath}/Configs";
                if (!Directory.Exists(_configPath))
                {
                    Directory.CreateDirectory(_configPath);
                }

                // 保存Prefab路径
                if (!Directory.Exists($"{_filePath}/Prefabs"))
                {
                    Directory.CreateDirectory($"{_filePath}/Prefabs");
                }
                // 删减路径，只保留/Assets及后面的
                _prefabPath = _filePath.Replace(Application.dataPath.Replace("/Assets", "") + "/", "") + "/Prefabs";
                AssetDatabase.Refresh();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("场景对象:");
            _targetGo = EditorGUILayout.ObjectField(_targetGo, typeof(GameObject), true) as GameObject;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("保存为相对路径：");
            _isRelative = (GUILayout.Toggle(_isRelative, "相对路径"));
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("生成"))
            {
                if (string.IsNullOrEmpty(_prefabPath))
                {
                    EditorUtility.DisplayDialog("提示", "请选择路径！", "确定");
                    return;
                }

                if (_targetGo == null)
                {
                    EditorUtility.DisplayDialog("提示","请选择场景对象！","确定");
                    return;
                }
                // 生成场景配置文件
                var editorSceneConfig = Generate(_targetGo.transform, _isRelative);
                File.WriteAllText(Path.Combine(_configPath,$"{_targetGo.name}.json"), JsonConvert.SerializeObject(editorSceneConfig), Encoding.UTF8);
                
                // 生成收集配置文件
                if (_configCollect == null)
                { 
                    _configCollect = new ExportSceneConfigCollect();
                }
                if (_configCollect.Configs == null)
                {
                    _configCollect.Configs = new List<string>();
                }
                if (!_configCollect.Configs.Contains(_targetGo.name))
                {
                    _configCollect.Configs.Add(_targetGo.name);
                }
                File.WriteAllText(_configColletPath, JsonConvert.SerializeObject(_configCollect), Encoding.UTF8);

                EditorUtility.DisplayDialog("提示", "配置文件生成完毕", "确定");
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("清理冗余（导出之前先执行此操作）"))
            {
                Clear();
                EditorUtility.DisplayDialog("提示", "清理完毕", "确定");
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("获取Source，测试"))
            {
                var go = PrefabUtility.GetCorrespondingObjectFromSource<GameObject>(_targetGo);
                Debug.Log(go.name);
            }
        }

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        private ExportSceneConfig Generate(Transform transform,bool isRelative)
        {
            var editorSceneConfig = new ExportSceneConfig
            {
                GameObjectName = transform.gameObject.name,
                ActiveSelf = transform.gameObject.activeSelf,
                // 本地坐标
                Local = new ExportSceneConfigTransform()
            };
            editorSceneConfig.Local.Position = transform.localPosition.ToString("F5");
            editorSceneConfig.Local.Rotation = transform.localEulerAngles.ToString("F5");
            editorSceneConfig.Local.Scale = transform.localScale.ToString("F5");
            // 世界坐标
            if (transform == _targetGo.transform)
            {
                editorSceneConfig.World = new ExportSceneConfigTransform
                {
                    Position = transform.position.ToString("F5"),
                    Rotation = transform.eulerAngles.ToString("F5"),
                    Scale = transform.localScale.ToString("F5")
                };
            }
            // 触发器
            editorSceneConfig.Triggers = new List<ExportSceneConfigTrigger>();
            foreach (var collider in transform.GetComponents<Collider>())
            {
                editorSceneConfig.Triggers.Add(GetTriggerData(collider));
            }
            // 生成Prefab
            if (transform.GetComponent<MeshFilter>())
            {
                // 如果已经是prefab，分两种情况
                // 1.本身是prefab，此时文件夹中应当存在该对象的prefab
                // 2.本身不是prefab，而是某一个prefab的子项，此时该文件夹中没有该对象的prefab
                if (PrefabUtility.IsPartOfAnyPrefab(transform.gameObject))
                {
                    // 获取源prefab
                    var sourcePrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource<GameObject>(transform.gameObject);
                    // 根据本地是否有文件判断是哪种情况
                    if (PrefabIsExists(sourcePrefab))
                    {
                        editorSceneConfig.PrefabPath = GetPrefabPath(sourcePrefab, isRelative);
                    }
                    else
                    {
                        // Clone 一份Prefab
                        var go = GameObject.Instantiate(sourcePrefab) as GameObject;
                        var prefabPath = GetAssetsPrefabPath(sourcePrefab);
                        // 保存到Prefab
                        PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
                        // 销毁clone出的实例
                        DestroyImmediate(go);
                        editorSceneConfig.PrefabPath = GetPrefabPath(sourcePrefab, isRelative);
                    }
                }
                else
                {
                    var prefabPath = GetAssetsPrefabPath(transform.gameObject);
                    PrefabUtility.SaveAsPrefabAsset(transform.gameObject, prefabPath);
                    editorSceneConfig.PrefabPath = GetPrefabPath(transform.gameObject, isRelative);
                }
            }
            // 子物体
            editorSceneConfig.Childs = new List<ExportSceneConfig>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                editorSceneConfig.Childs.Add(Generate(child, isRelative));
            }

            return editorSceneConfig;
        }

        /// <summary>
        /// 序列化触发器
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        public ExportSceneConfigTrigger GetTriggerData(Collider collider)
        {
            var editorSceneConfigTrigger = new ExportSceneConfigTrigger
            {
                TriggerName = collider.GetType().Name
            };
            switch (editorSceneConfigTrigger.TriggerName)
            {
                case nameof(BoxCollider):
                    var boxCollider = collider as BoxCollider;
                    var boxtriggerData = new ExportSceneConfigTriggerBox()
                    {
                        Center = boxCollider.center.ToString("F5"),
                        Size = boxCollider.size.ToString("F5")
                    };
                    editorSceneConfigTrigger.Data = JsonConvert.SerializeObject(boxtriggerData);
                    break;
                case nameof(CapsuleCollider):
                    var capsuleCollider = collider as CapsuleCollider;
                    var capsuleTriggerData = new ExportSceneConfigTriggerCapsule()
                    {
                        Center = capsuleCollider.center.ToString("F5"),
                        Radius = capsuleCollider.radius,
                        Height = capsuleCollider.height,
                        Direction = capsuleCollider.direction
                    };
                    editorSceneConfigTrigger.Data = JsonConvert.SerializeObject(capsuleTriggerData);
                    break;
                case nameof(SphereCollider):
                    var sphereCollider = collider as SphereCollider;
                    var sphereTriggerData = new ExportSceneConfigTriggerSphere()
                    {
                        Center = sphereCollider.center.ToString("F5"),
                        Radius = sphereCollider.radius
                    };
                    editorSceneConfigTrigger.Data = JsonConvert.SerializeObject(sphereTriggerData);
                    break;
            }
            return editorSceneConfigTrigger;
        }

        /// <summary>
        /// 清除冗余Prefab
        /// </summary>
        private void Clear()
        {
            if (string.IsNullOrEmpty(_configColletPath))
            {
                EditorUtility.DisplayDialog("提示", "请选择路径", "确定");
                return;
            }
            if (!File.Exists(_configColletPath))
            {
                EditorUtility.DisplayDialog("提示", "Collect文件不存在，已跳过清理步骤", "确定");
                return;
            }
            var collectTxt = File.ReadAllText(_configColletPath);
            if (string.IsNullOrEmpty(collectTxt))
            {
                EditorUtility.DisplayDialog("提示", "Collect文件为空，已跳过清理步骤", "确定");
                return;
            }
            var configCollect = JsonConvert.DeserializeObject<ExportSceneConfigCollect>(collectTxt);
            if (configCollect.Configs == null)
            {
                EditorUtility.DisplayDialog("提示", "Collect文件中不包含任何配置项，已跳过清理步骤", "确定");
                return;
            }

            // 收集配置文件中的prefab路径
            var prefabNames = new List<string>();
            foreach (var configName in configCollect.Configs)
            {
                var conigPath = $"{_configPath}/{configName}.json";
                if (!File.Exists(conigPath))
                {
                    continue;
                }
                var configTxt = File.ReadAllText($"{_configPath}/{configName}.json");
                if (string.IsNullOrEmpty(configTxt))
                {
                    continue;
                }
                var config = JsonConvert.DeserializeObject<ExportSceneConfig>(configTxt);
                GetAllPrefabPath(config, prefabNames);
            }

            // 收集文件夹中的Prefab路径
            var files = Directory.GetFiles(_prefabPath);
            
            // 对比文件夹中的文件和配置文件中的文件，若配置文件中没有该文件，说明该文件冗余，删除该文件
            foreach (var file in files)
            {
                if (Path.GetExtension(file).Equals(".prefab"))
                {
                    var fileName = Path.GetFileName(file);
                    if (!prefabNames.Contains(fileName))
                    {
                        File.Delete(file);
                    }
                }
            }
        }

        /// <summary>
        /// 获取配置文件中的所有prefab路径
        /// </summary>
        /// <param name="editorSceneConfig">配置文件对象</param>
        /// <param name="paths">路径集合</param>
        private void GetAllPrefabPath(ExportSceneConfig editorSceneConfig,List<string> paths)
        {
            if (editorSceneConfig.Childs != null)
            {
                foreach (var child in editorSceneConfig.Childs)
                {
                    GetAllPrefabPath(child,paths);
                }
            }
            if (!string.IsNullOrEmpty(editorSceneConfig.PrefabPath))
            {
                paths.Add(Path.GetFileName(editorSceneConfig.PrefabPath));
            }
        }

        /// <summary>
        /// 判断Prefab文件是否存在
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        private bool PrefabIsExists(GameObject prefab)
        {
            var files = Directory.GetFiles(GetPrefabFullPath());
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                if(prefab.name.Equals(fileName))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 获取Prefab的路径
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="isRelative">是否获取相对路径</param>
        /// <returns></returns>
        private string GetPrefabPath(GameObject gameObject, bool isRelative)
        {
            return isRelative ? GetRelativePath(gameObject) : GetAssetsPrefabPath(gameObject);
        }

        /// <summary>
        /// 获取Prefab的保存路径，注意这不是真实路径，是从/Assets开始的路径
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        private string GetAssetsPrefabPath(GameObject gameObject)
        {
            return Path.Combine(_prefabPath, $"{gameObject.name}.prefab");
        }

        /// <summary>
        /// 获取Prefab的相对路径
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        private string GetRelativePath(GameObject gameObject)
        {
            return Path.Combine("Prefabs", $"{gameObject.name}.prefab");
        }

        /// <summary>
        /// 获取prefab文件夹的完整路径
        /// </summary>
        /// <returns></returns>
        private string GetPrefabFullPath()
        {
            return Path.Combine(System.Environment.CurrentDirectory, _prefabPath);
        }
    }
}