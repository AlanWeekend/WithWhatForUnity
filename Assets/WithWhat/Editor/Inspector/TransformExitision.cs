using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace WithWhat.Editor
{
    [CustomEditor(typeof(Transform))]
    public class TransformExitision : UnityEditor.Editor
    {
        private Transform _transform;
        private UnityEditor.Editor _editor; 
        private bool _foldoutType;

        private void OnEnable()
        {
            _transform = (Transform)target;//被检查的对象是Transform；
            _editor = CreateEditor(target, Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.TransformInspector", true));
        }
        public override void OnInspectorGUI()
        {
            _editor.OnInspectorGUI();
            //绘制折叠框
            _foldoutType = EditorGUILayout.Foldout(_foldoutType, "快捷操作");
            //绘制成功就继续绘制
            if (_foldoutType)
            {
                //垂直布局
                EditorGUILayout.BeginVertical();
                // 水平布局
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("复制世界坐标"))
                {
                    TextEditor textEd = new TextEditor();
                    var str = $"{_transform.position.x},{_transform.position.y},{_transform.position.z}";
                    textEd.text = str;
                    textEd.OnFocus();
                    textEd.Copy();
                }
                if (GUILayout.Button("复制本地坐标"))
                {
                    TextEditor textEd = new TextEditor();
                    var str = $"{_transform.localPosition.x},{_transform.localPosition.y},{_transform.localPosition.z}";
                    textEd.text = str;
                    textEd.OnFocus();
                    textEd.Copy();
                }
                EditorGUILayout.EndHorizontal();
                // 水平布局
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("复制世界欧拉角"))
                {
                    TextEditor textEd = new TextEditor();
                    var str = $"{_transform.eulerAngles.x},{_transform.eulerAngles.y},{_transform.eulerAngles.z}";
                    textEd.text = str;
                    textEd.OnFocus();
                    textEd.Copy();
                }
                if (GUILayout.Button("复制本地欧拉角"))
                {
                    TextEditor textEd = new TextEditor();
                    var str = $"{_transform.localEulerAngles.x},{_transform.localEulerAngles.y},{_transform.localEulerAngles.z}";
                    textEd.text = str;
                    textEd.OnFocus();
                    textEd.Copy();
                }
                EditorGUILayout.EndHorizontal();
                // 水平布局
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("复制本地旋转"))
                {
                    TextEditor textEd = new TextEditor();
                    var str = $"{_transform.localScale.x},{_transform.localScale.y},{_transform.localScale.z}";
                    textEd.text = str;
                    textEd.OnFocus();
                    textEd.Copy();
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
        }


    }
}