using UnityEngine;

namespace WithWhat.Utils
{
    public static class VectorUtils
    {
        /// <summary>
        /// 字符串转v3
        /// </summary>
        /// <param name="position">字符串 "(1,1,1)" 或 "1,1,1"</param>
        /// <returns></returns>
        public static Vector3 StringToVector3(string position)
        {
            if (string.IsNullOrEmpty(position))
            {
                return Vector3.zero;
            }
            var pos = position.Replace("(", "").Replace(")", "").Split(',');
            return new Vector3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));
        }

        /// <summary>
        /// 字符串转v2
        /// </summary>
        /// <param name="position">字符串 "(1,1)" 或 "1,1"</param>
        /// <returns></returns>
        public static Vector2 StringToVector2(string position)
        {
            if (string.IsNullOrEmpty(position))
            {
                return Vector2.zero;
            }
            var pos = position.Replace("(", "").Replace(")", "").Split(',');
            return new Vector2(float.Parse(pos[0]), float.Parse(pos[1]));
        }
    }
}