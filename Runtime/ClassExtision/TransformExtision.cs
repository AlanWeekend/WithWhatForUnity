using System.Collections.Generic;
using UnityEngine;

namespace ZCCUtils.ClassExtision
{
    public static class TransformExtision
    {
        /// <summary>
        /// 获取所有的子物体，不包含孙物体
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static List<Transform> GetChilds(this Transform transform)
        {
            var childs = new List<Transform>();
            for (int i = 0; i < transform.childCount; i++)
            {
                childs.Add(transform.GetChild(i));
            }
            return childs;
        }

        /// <summary>
        /// 设置所有子物体的active，不包含孙物体
        /// </summary>
        /// <param name="transform"></param>
        public static void SetChildsActive(this Transform transform, bool value)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(value);
            }
        }
    }
}