using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace WithWhat.Editor
{
    public class EditorWindowBatchUpdateGameObjectName : EditorWindow
    {
        private enum UpdateType
        {
            AddToBeginning,
            Replace,
            AddToEnd,
            DeleteFromDeginning,
            DeleteFromEnd
        }

        [SerializeField]
        private List<GameObject> _targetGo = new List<GameObject>();
        private SerializedObject _serializedObject;
        private SerializedProperty _serializedProperty;
        private string _targetName;
        private string _name;
        private UpdateType _updateType;

        private void OnEnable()
        {
            _serializedObject = new SerializedObject(this);
            _serializedProperty = _serializedObject.FindProperty("_targetGo");
        }

        public void DrawGUI()
        {
            _serializedObject.Update();
            WithWhatEditorWindowUtil.DrawTitle("批量修改目标游戏对象的名字");
            WithWhatEditorWindowUtil.DrawOneContent("说明", "将所有目标对象的名字同一修改");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("目标游戏对象");
            EditorGUILayout.PropertyField(_serializedProperty, true);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("修改名称");
            _targetName = EditorGUILayout.TextField(_targetName);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("修改类型");
            _updateType = (UpdateType)EditorGUILayout.EnumPopup(_updateType);
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("修改"))
            {
                if (string.IsNullOrEmpty(_targetName) || _targetGo == null)
                {
                    EditorUtility.DisplayDialog("提示", "目标游戏对象或目标名字不能为空！", "确定");
                    return;
                }
                UpdateGameObjectName();
                EditorUtility.DisplayDialog("提示", "替换完成", "确定");
            }
        }

        private void UpdateGameObjectName()
        {
            foreach (var go in _targetGo)
            {
                switch (_updateType)
                {
                    case UpdateType.AddToBeginning:
                        go.gameObject.name = $"{_targetName}{go.gameObject.name}";
                        break;
                    case UpdateType.Replace:
                        go.gameObject.name = _targetName;
                        break;
                    case UpdateType.AddToEnd:
                        go.gameObject.name = $"{go.gameObject.name}{_targetName}";
                        break;
                    case UpdateType.DeleteFromDeginning:
                        if (go.gameObject.name.StartsWith(_targetName))
                        {
                            go.gameObject.name = go.gameObject.name.Remove(0, _targetName.Length);
                        }
                        break;
                    case UpdateType.DeleteFromEnd:
                        var name = go.gameObject.name;
                        if (name.EndsWith(_targetName))
                        {
                            go.gameObject.name = name.Remove(name.Length - _targetName.Length, _targetName.Length);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}