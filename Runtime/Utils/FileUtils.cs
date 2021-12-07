using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace WithWhat.Utils
{
    public class FileUtils
    {
        /// <summary>
        /// ����ļ���
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool ClearDirectory(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path)
                    || !Directory.Exists(path))
                {
                    return true;  // �������Ϊ�գ�����Ϊ�ѳɹ����
                }
                // ɾ����ǰ�ļ����������ļ�
                foreach (string strFile in Directory.GetFiles(path))
                {
                    File.Delete(strFile);
                }
                // ɾ����ǰ�ļ������������ļ���(�ݹ�)
                foreach (string strDir in Directory.GetDirectories(path))
                {
                    Directory.Delete(strDir, true);
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("��� {0} �쳣, ��Ϣ:{1}, ��ջ:{2}"
                    , path, ex.Message, ex.StackTrace));
                return false;
            }
        }

        /// <summary>
        /// ȥ��·���еĺ�׺
        /// </summary>
        /// <param name="path">·��</param>
        /// <returns></returns>
        public static string RemovePathExtision(string path)
        {
            if (path.LastIndexOf(".") != -1)
            {
                return path.Remove(path.LastIndexOf("."), path.Length - path.LastIndexOf("."));
            }
            return path;
        }
    }
}