using UnityEngine;

namespace WithWhat.Utils
{
    public static class Vector3Utils
    {
        /// <summary>
        /// 字符串转v3
        /// </summary>
        /// <param name="position">字符串 "(1,1,1)" 或 "1,1,1"</param>
        /// <returns></returns>
        public static Vector3 StringToVector3(string position)
        {
            var pos = position.Replace("(", "").Replace(")", "").Split(',');
            return new Vector3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));
        }
    }
}