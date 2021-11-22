using System.Collections.Generic;
using UnityEngine;

namespace WithWhat.ClassExtision
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

        /// <summary>
        /// 设置物体的世界坐标为鼠标所在位置
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="camera">相机</param>
        /// <param name="targetGameObjectPosition">目标物体(参考物)坐标</param>
        /// <param name="offset">偏移量</param>
        /// <returns>鼠标位置对应的世界坐标</returns>
        public static void SetPostionToMousePosition(this Transform transform, Camera camera, Vector3 targetGameObjectPosition,Vector3 offset)
        {
            //将对象坐标换成屏幕坐标
            Vector3 pos = camera.WorldToScreenPoint(targetGameObjectPosition);
            //让鼠标的屏幕坐标与对象坐标一致
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, pos.z) + offset;
            transform.position = camera.ScreenToWorldPoint(mousePos);
        }

        /// <summary>
        /// 根据名称查找子物体（递归）
        /// </summary>
        /// <param name="root"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Transform FindChildByName(this Transform root, string name)
        {
            Transform child = root.Find(name);
            if (child != null)
                return child;

            Transform go = null;
            for (int i = 0; i < root.childCount; i++)
            {
                child = root.GetChild(i);
                go = FindChildByName(child, name);
                if (go != null)
                    return go;
            }
            return null;
        }
    }
}