using System;
using System.IO;
using UnityEngine;

namespace WithWhat.Editor
{
    public class FileUtils
    {
        /// <summary>
        /// 清空文件夹
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
                    return true;  // 如果参数为空，则视为已成功清空
                }
                // 删除当前文件夹下所有文件
                foreach (string strFile in Directory.GetFiles(path))
                {
                    File.Delete(strFile);
                }
                // 删除当前文件夹下所有子文件夹(递归)
                foreach (string strDir in Directory.GetDirectories(path))
                {
                    Directory.Delete(strDir, true);
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Format("清空 {0} 异常, 消息:{1}, 堆栈:{2}"
                    , path, ex.Message, ex.StackTrace));
                return false;
            }
        }
    }
}