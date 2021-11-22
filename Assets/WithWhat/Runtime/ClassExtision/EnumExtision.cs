using System;
using System.Collections.Generic;
using System.Linq;

namespace WithWhat.ClassExtision
{
    public static class EnumExtision
    {
        /// <summary>
        /// 获取枚举所有名称
        /// </summary>
        /// <param name="enumType">枚举类型typeof(T)</param>
        /// <returns>枚举名称列表</returns>
        public static List<string> GetEnumNamesList(this Type enumType)
        {
            return Enum.GetNames(enumType).ToList();
        }
    }
}