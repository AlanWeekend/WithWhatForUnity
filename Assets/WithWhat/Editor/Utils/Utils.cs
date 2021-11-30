using UnityEngine;

namespace WithWhat.Editor
{
    public class Utils
    {
        /// <summary>
        /// 复制到剪切板
        /// </summary>
        /// <param name="str">复制的字符串</param>
        public static void CopyToClipboard(string str)
        {
            TextEditor textEd = new TextEditor();
            textEd.text = str;
            textEd.OnFocus();
            textEd.Copy();
        }
    }
}