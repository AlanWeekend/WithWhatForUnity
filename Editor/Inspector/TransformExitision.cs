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
                    var eulerAngles = GetEulerAngles(_transform);
                    TextEditor textEd = new TextEditor();
                    var str = $"{eulerAngles.x},{eulerAngles.y},{eulerAngles.z}";
                    textEd.text = str;
                    textEd.OnFocus();
                    textEd.Copy();
                }
                if (GUILayout.Button("复制本地欧拉角"))
                {
                    var localEulerAngles = GetLocalEulerAngles(_transform);
                    TextEditor textEd = new TextEditor();
                    var str = $"{localEulerAngles.x},{localEulerAngles.y},{localEulerAngles.z}";
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

        /// <summary>
        /// 获取本地欧拉角
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public Vector3 GetLocalEulerAngles(Transform transform)
        {
            return GetInspectorEulerAnglesValueMethod(transform, Vector3.zero, true);
        }

        /// <summary>
        /// 获取世界欧拉角
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public Vector3 GetEulerAngles(Transform transform)
        {
            return GetInspectorEulerAnglesValueMethod(transform, Vector3.zero, false);
        }

        /// <summary>
        /// 获取面板真实欧拉角
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="childEulerAngles">子物体的欧拉角，递归获取世界欧拉角时用</param>
        /// <param name="isLocal">true 获取本地欧拉角，false获取世界欧拉角</param>
        /// <returns></returns>
        private Vector3 GetInspectorEulerAnglesValueMethod(Transform transform, Vector3 childEulerAngles, bool isLocal)
        {
            // 获取原生值
            System.Type transformType = transform.GetType();
            PropertyInfo m_propertyInfo_rotationOrder = transformType.GetProperty("rotationOrder", BindingFlags.Instance | BindingFlags.NonPublic);
            object m_OldRotationOrder = m_propertyInfo_rotationOrder.GetValue(transform, null);
            MethodInfo m_methodInfo_GetLocalEulerAngles = transformType.GetMethod($"GetLocalEulerAngles", BindingFlags.Instance | BindingFlags.NonPublic);
            object value = m_methodInfo_GetLocalEulerAngles.Invoke(transform, new object[] { m_OldRotationOrder });
            string temp = value.ToString();
            //将字符串第一个和最后一个去掉
            temp = temp.Remove(0, 1);
            temp = temp.Remove(temp.Length - 1, 1);
            //用‘，’号分割
            string[] tempVector3;
            tempVector3 = temp.Split(',');
            //将分割好的数据传给Vector3
            Vector3 vector3 = new Vector3(float.Parse(tempVector3[0]), float.Parse(tempVector3[1]), float.Parse(tempVector3[2]));
            // 获取世界欧拉角时，递归父物体累加旋转角度
            if (!isLocal)
            {
                if (transform.parent != null)
                {
                    return vector3 + GetInspectorEulerAnglesValueMethod(transform.parent, vector3, isLocal);
                }
                else
                {
                    return vector3;
                }
            }
            return vector3;
        }
    }
}