using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace WithWhat.Utils
{
    public class UIUtils
    {
        /// <summary>
        /// ǿ��ˢ�²���
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static IEnumerator ForceUpdateLayout(RectTransform rect)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
            yield return new WaitForEndOfFrame();
            while (rect.rect.width == 0)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}