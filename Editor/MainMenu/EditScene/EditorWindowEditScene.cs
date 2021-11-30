using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace WithWhat.Editor
{
    public class EditorWindowEditScene : EditorWindow
    {

        private GameObject _targetGo;
        private string _filePath;
        private string _prefabPath;

        public void DrawGUI()
        {
            WithWhatEditorWindowUtil.DrawTitle("场景导出");
            WithWhatEditorWindowUtil.DrawOneContent("说明", "生成场景配置文件及prefab，注意以下几点\r\n" +
                "1.Prefab不能生成到StreamingAsset文件下,否则无法加载到依赖关系,即使实例化到场景里,也仅是一个有预制体名字的空物体\r\n" +
                "2.已经绑定Prefab的物体及其子物体不能再生成Prefab");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"保存路径:{_filePath}");
            if (GUILayout.Button("选择路径"))
            {
                _filePath = EditorUtility.SaveFolderPanel("选择路径", Application.dataPath, "");
                // 删减路径，只保留/Assets及后面的
                _prefabPath = _filePath.Replace(Application.dataPath.Replace("/Assets", "") + "/", "");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("场景对象");
            _targetGo = EditorGUILayout.ObjectField(_targetGo, typeof(GameObject), true) as GameObject;
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

                // 生成之前先清空文件夹，防止冗余
                FileUtils.ClearDirectory(_filePath);
                // 生成
                var editorSceneConfig = Generate(_targetGo.transform);
                File.WriteAllText(Path.Combine(_filePath,"SceneData.json"), JsonConvert.SerializeObject(editorSceneConfig), Encoding.UTF8);
                EditorUtility.DisplayDialog("提示", "配置文件生成完毕", "确定");
                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        private EditorSceneConfig Generate(Transform transform)
        {
            var editorSceneConfig = new EditorSceneConfig();
            editorSceneConfig.DeviceCode = transform.gameObject.name;
            // 本地坐标
            editorSceneConfig.Local = new EditorSceneConfigTransform();
            editorSceneConfig.Local.Position = transform.localPosition.ToString();
            editorSceneConfig.Local.Rotation = transform.localEulerAngles.ToString();
            editorSceneConfig.Local.Scale = transform.localScale.ToString();
            // 触发器
            editorSceneConfig.Triggers = new List<EditorSceneConfigTrigger>();
            foreach (var collider in transform.GetComponents<Collider>())
            {
                editorSceneConfig.Triggers.Add(GetTriggerData(collider));
            }
            // 生成Prefab
            if (transform.GetComponent<MeshFilter>())
            {
                var prefabPath = Path.Combine(_prefabPath, transform.name + ".prefab");
                Debug.Log(prefabPath);
                PrefabUtility.SaveAsPrefabAsset(transform.gameObject, prefabPath);
                editorSceneConfig.PrefabPath = prefabPath;
            }
            // 子物体
            editorSceneConfig.Childs = new List<EditorSceneConfig>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                editorSceneConfig.Childs.Add(Generate(child));
            }

            return editorSceneConfig;
        }

        /// <summary>
        /// 序列化触发器
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        public EditorSceneConfigTrigger GetTriggerData(Collider collider)
        {
            var editorSceneConfigTrigger = new EditorSceneConfigTrigger();
            editorSceneConfigTrigger.TriggerName = collider.GetType().Name;
            switch (editorSceneConfigTrigger.TriggerName)
            {
                case "BoxCollider":
                    var boxCollider = collider as BoxCollider;
                    var boxtriggerData = new EditorSceneConfigTriggerBox()
                    {
                        Center = boxCollider.center.ToString(),
                        Size = boxCollider.size.ToString()
                    };
                    editorSceneConfigTrigger.Data = JsonConvert.SerializeObject(boxtriggerData);
                    break;
                case "CapsuleCollider":
                    var capsuleCollider = collider as CapsuleCollider;
                    var capsuleTriggerData = new EditorSceneConfigTriggerCapsule()
                    {
                        Center = capsuleCollider.center.ToString(),
                        Radius = capsuleCollider.radius,
                        Height = capsuleCollider.height,
                        Direction = capsuleCollider.direction
                    };
                    editorSceneConfigTrigger.Data = JsonConvert.SerializeObject(capsuleTriggerData);
                    break;
                case "SphereCollider":
                    var sphereCollider = collider as SphereCollider;
                    var sphereTriggerData = new EditorSceneConfigTriggerSphere()
                    {
                        Center = sphereCollider.center.ToString(),
                        Radius = sphereCollider.radius
                    };
                    editorSceneConfigTrigger.Data = JsonConvert.SerializeObject(sphereTriggerData);
                    break;
            }
            return editorSceneConfigTrigger;
        }
    }
}