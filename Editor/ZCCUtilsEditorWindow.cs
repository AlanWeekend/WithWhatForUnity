using UnityEditor;
using UnityEngine;

namespace ZCCUtils.Editor
{
    public class ZCCUtilsEditorWindow : EditorWindow
    {
        #region 数据成员
        private string[] tabNames = new string[] { "批量替换材质球" };
        private int selectedTabID;
        public static int FONTSIZE = 18;

        private EditorWindowBatchReplaceMaterials batchReplaceMaterials;
        #endregion

        #region 编辑器入口
        [MenuItem("ZCCUtils/Tools")]
        private static void ShowWindow()
        {
            var window = GetWindow<ZCCUtilsEditorWindow>();
            window.titleContent = new GUIContent("ZCCUtils");
            window.Show();
        }
        #endregion

        #region OnEnable/OnDisable
        private void OnEnable()
        {
            batchReplaceMaterials = ScriptableObject.CreateInstance<EditorWindowBatchReplaceMaterials>();
        }
        #endregion

        private void OnGUI()
        {
            #region 左侧边栏
            EditorGUILayout.BeginHorizontal();
            float _width = 150;
            float _height = position.height - 30;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.MaxWidth(_width), GUILayout.MinHeight(_height));
            // 功能选择
            selectedTabID = GUILayout.SelectionGrid(selectedTabID, tabNames, 1);
            EditorGUILayout.EndVertical();
            #endregion

            #region 右侧功能区
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.MinWidth(position.width - _width), GUILayout.MinHeight(_height));
            DrawMainUI(selectedTabID);
            EditorGUILayout.EndVertical();
            #endregion
            EditorGUILayout.EndHorizontal();

            Repaint();
        }

        #region 绘制分级UI
        void DrawMainUI(int selectedTabID)
        {
            switch (selectedTabID)
            {
                case 0:
                    batchReplaceMaterials.DrawGUI();
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}