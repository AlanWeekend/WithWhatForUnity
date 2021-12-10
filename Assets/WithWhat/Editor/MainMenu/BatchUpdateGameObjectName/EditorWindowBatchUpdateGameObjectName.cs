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
            WithWhatEditorWindowUtil.DrawTitle("�����޸�Ŀ����Ϸ���������");
            WithWhatEditorWindowUtil.DrawOneContent("˵��", "������Ŀ����������ͬһ�޸�");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Ŀ����Ϸ����");
            EditorGUILayout.PropertyField(_serializedProperty, true);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("�޸�����");
            _targetName = EditorGUILayout.TextField(_targetName);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("�޸�����");
            _updateType = (UpdateType)EditorGUILayout.EnumPopup(_updateType);
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("�޸�"))
            {
                if (string.IsNullOrEmpty(_targetName) || _targetGo == null)
                {
                    EditorUtility.DisplayDialog("��ʾ", "Ŀ����Ϸ�����Ŀ�����ֲ���Ϊ�գ�", "ȷ��");
                    return;
                }
                UpdateGameObjectName();
                EditorUtility.DisplayDialog("��ʾ", "�滻���", "ȷ��");
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