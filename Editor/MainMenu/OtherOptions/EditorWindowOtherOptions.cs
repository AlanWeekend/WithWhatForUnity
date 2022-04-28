using System.IO;
using UnityEditor;
using UnityEngine;

namespace WithWhat.Editor
{
    public class EditorWindowOtherOptions : EditorWindow
    {
        /// <summary>
        /// 项目路径
        /// </summary>
        private string projectPath = System.Environment.CurrentDirectory;
        /// <summary>
        /// 插件路径
        /// </summary>
        private string pluginsPath = "\\Library\\PackageCache";
        /// <summary>
        /// 插件包名
        /// </summary>
        private string packageName = "com.zcc.withwhat";
        /// <summary>
        /// 插件文件夹名
        /// </summary>
        private string packageDirectoryName;
        /// <summary>
        /// 脚本路径
        /// </summary>
        private string scriptPath = "\\Runtime\\Net\\SignalR";
        /// <summary>
        /// 脚本名
        /// </summary>
        private string scriptName = "SignalRClient";
        /// <summary>
        /// 脚本在项目中的路径
        /// </summary>
        private string scriptFullPathInProject;
        /// <summary>
        /// 脚本在Package中的路径
        /// </summary>
        private string scriptFullPathInPackasges;
        /// <summary>
        /// 脚本路径
        /// </summary>
        private string scriptSourceFullPath;
        private string scriptDstFullPath;
        /// <summary>
        /// 是否打开
        /// </summary>
        private bool isStart;

        private bool isToggle;

        private void Awake()
        {
            scriptFullPathInProject = $"{projectPath}\\Assets\\WithWhat{scriptPath}";
            scriptDstFullPath = $"{projectPath}\\Assets\\Plugins\\SignalR";
            Debug.Log(scriptDstFullPath);

            // 在插件项目中运行
            if (Directory.Exists(scriptFullPathInProject))
            {
                scriptSourceFullPath = scriptFullPathInProject;
            }
            // 在Package中运行
            else
            {
                var dirs = Directory.GetDirectories($"{projectPath}{pluginsPath}");

                foreach (var dir in dirs)
                {
                    if (dir.Contains("@"))
                    {
                        if (dir.Split('@')[0].EndsWith(packageName))
                        {
                            packageDirectoryName = dir;
                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(packageDirectoryName))
                {
                    Debug.LogError($"{packageName} 插件路径不存在！！！");
                    return;
                }

                scriptSourceFullPath = $"{projectPath}{pluginsPath}\\{packageDirectoryName}{scriptPath}";
            }

            isStart = File.Exists($"{scriptDstFullPath}\\{scriptName}.cs");
            isToggle = isStart;
        }

        public void DrawGUI()
        {
            WithWhatEditorWindowUtil.DrawTitle("其他选项");
            WithWhatEditorWindowUtil.DrawOneContent(@"SignalR说明", "启用前请保证已在项目中添加Microsoft.AspNetCore.SignalR.Client.dll及所需的依赖");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("SignalRClient:");
            if (isToggle != isStart)
            {
                // 打开
                if (isToggle)
                {
                    if (!File.Exists(scriptDstFullPath))
                    {
                        Directory.CreateDirectory(scriptDstFullPath);
                    }
                    File.Copy($"{scriptSourceFullPath}\\{scriptName}.txt", $"{scriptDstFullPath}\\{scriptName}.cs");
                }
                // 关闭
                else
                {
                    File.Delete($"{scriptDstFullPath}\\{scriptName}.cs");
                }
                isStart = isToggle;
                AssetDatabase.Refresh();
            }
            isToggle = GUILayout.Toggle(isToggle, "启用");
            if (GUILayout.Button("下载Microsoft.AspNetCore.SignalR.Client.dll"))
            {
                Application.OpenURL("https://yongshen.me/WithWhatForUnity/Net/SignalRClient/");
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}