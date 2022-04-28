using System.IO;
using UnityEditor;
using UnityEngine;

namespace WithWhat.Editor
{
    public class EditorWindowOtherOptions : EditorWindow
    {
        /// <summary>
        /// ��Ŀ·��
        /// </summary>
        private string projectPath = System.Environment.CurrentDirectory;
        /// <summary>
        /// ���·��
        /// </summary>
        private string pluginsPath = "\\Library\\PackageCache";
        /// <summary>
        /// �������
        /// </summary>
        private string packageName = "com.zcc.withwhat";
        /// <summary>
        /// ����ļ�����
        /// </summary>
        private string packageDirectoryName;
        /// <summary>
        /// �ű�·��
        /// </summary>
        private string scriptPath = "\\Runtime\\Net\\SignalR";
        /// <summary>
        /// �ű���
        /// </summary>
        private string scriptName = "SignalRClient";
        /// <summary>
        /// �ű�����Ŀ�е�·��
        /// </summary>
        private string scriptFullPathInProject;
        /// <summary>
        /// �ű���Package�е�·��
        /// </summary>
        private string scriptFullPathInPackasges;
        /// <summary>
        /// �ű�·��
        /// </summary>
        private string scriptFullPath;
        /// <summary>
        /// �Ƿ��
        /// </summary>
        private bool isStart;

        private bool isToggle;

        private void Awake()
        {
            scriptFullPathInProject = $"{projectPath}\\Assets\\WithWhat{scriptPath}";

            // �ڲ����Ŀ������
            if (Directory.Exists(scriptFullPathInProject))
            {
                scriptFullPath = scriptFullPathInProject;
            }
            // ��Package������
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
                    Debug.LogError($"{packageName} ���·�������ڣ�����");
                    return;
                }

                scriptFullPath = $"{projectPath}{pluginsPath}\\{packageDirectoryName}{scriptPath}";
            }

            isStart = File.Exists($"{scriptFullPath}\\{scriptName}.cs");
            isToggle = isStart;
        }

        public void DrawGUI()
        {
            WithWhatEditorWindowUtil.DrawTitle("����ѡ��");
            WithWhatEditorWindowUtil.DrawOneContent(@"SignalR˵��", "����ǰ�뱣֤������Ŀ�����Microsoft.AspNetCore.SignalR.Client.dll�����������");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("SignalRClient:");
            if (isToggle != isStart)
            {
                // ��
                if (isToggle)
                {
                    File.Move($"{scriptFullPath}\\{scriptName}.txt", $"{scriptFullPath}\\{scriptName}.cs");
                }
                // �ر�
                else
                {
                    File.Move($"{scriptFullPath}\\{scriptName}.cs", $"{scriptFullPath}\\{scriptName}.txt");
                }
                isStart = isToggle;
                AssetDatabase.Refresh();
            }
            isToggle = GUILayout.Toggle(isToggle, "����");
            if (GUILayout.Button("����Microsoft.AspNetCore.SignalR.Client.dll"))
            {
                Application.OpenURL("https://yongshen.me/WithWhatForUnity/Net/SignalRClient/");
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}