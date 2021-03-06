using UnityEditor;
using UnityEngine;

namespace WithWhat.Editor
{
    public class WithWhatEditorWindow : EditorWindow
    {
        #region 数据成员
        private string[] tabNames = new string[] { "批量替换材质球", "场景导出","批量改名","其他选项" };
        private int selectedTabID;
        public static int FONTSIZE = 18;

        private EditorWindowBatchReplaceMaterials _batchReplaceMaterials;
        private EditorWindowExportScene _editorWindowExportScene;
        private EditorWindowBatchUpdateGameObjectName _batchUpdateGameObjectName;
        private EditorWindowOtherOptions _otherOptions;
        #endregion

        #region 编辑器入口
        [MenuItem("WithWhat/Tools")]
        private static void ShowWindow()
        {
            var window = GetWindow<WithWhatEditorWindow>();
            window.titleContent = new GUIContent("WithWhat");
            window.Show();
        }
        #endregion

        #region OnEnable/OnDisable
        private void OnEnable()
        {
            _batchReplaceMaterials = ScriptableObject.CreateInstance<EditorWindowBatchReplaceMaterials>();
            _editorWindowExportScene = ScriptableObject.CreateInstance<EditorWindowExportScene>();
            _batchUpdateGameObjectName = ScriptableObject.CreateInstance<EditorWindowBatchUpdateGameObjectName>();
            _otherOptions = ScriptableObject.CreateInstance<EditorWindowOtherOptions>();
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
                    _batchReplaceMaterials.DrawGUI();
                    break;
                case 1:
                    _editorWindowExportScene.DrawGUI();
                    break;
                case 2:
                    _batchUpdateGameObjectName.DrawGUI();
                    break;
                case 3:
                    _otherOptions.DrawGUI();
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}