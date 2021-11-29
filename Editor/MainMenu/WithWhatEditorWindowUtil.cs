using UnityEditor;
using UnityEngine;

namespace WithWhat.Editor
{
    public class WithWhatEditorWindowUtil : MonoBehaviour
    {
        /// <summary>
        /// 绘制一条内容
        /// </summary>
        /// <param name="str">大标题内容</param>
        /// <param name="message">小标题内容</param>
        public static void DrawOneContent(string str, string message = null)
        {
            //主按钮样式
            GUIStyle style01 = new GUIStyle("label");
            style01.alignment = TextAnchor.MiddleLeft;
            style01.wordWrap = false;
            style01.fontStyle = FontStyle.Bold;
            style01.fontSize = WithWhatEditorWindow.FONTSIZE;

            //说明样式
            GUIStyle style02 = new GUIStyle("label");
            style02.wordWrap = true;
            style02.richText = true;
            style02.fontSize = WithWhatEditorWindow.FONTSIZE - 4;

            EditorGUILayout.BeginVertical(new GUIStyle("Box"));
            EditorGUILayout.TextArea(str, style01);
            EditorGUILayout.TextArea(message, style02);
            EditorGUILayout.EndVertical();
        }

        public static void DrawTitle(string str)
        {
            EditorGUILayout.LabelField(str, EditorStyles.miniButton);
        }
    }
}