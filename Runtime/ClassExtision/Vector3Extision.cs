using UnityEngine;

namespace WithWhat.ClassExtision
{
    public static class Vector3Extision
    {
        /// <summary>
        /// 计算两个点的中点
        /// </summary>
        /// <param name="vector3">当前点</param>
        /// <param name="to">下一个点</param>
        /// <returns></returns>
        public static Vector3 Middle(this Vector3 vector3, Vector3 to)
        {
            return new Vector3((vector3.x + to.x) / 2, (vector3.y + to.y) / 2, (vector3.z + to.z) / 2);
        }
    }
}