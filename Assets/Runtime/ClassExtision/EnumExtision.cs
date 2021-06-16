using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ZCCUtils.ClassExtision
{
    public static class EnumExtision
    {
        /// <summary>
        /// 获取枚举所有名称
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static List<string> GetEnumNamesList<T>(this T @enum) where T : Enum
        {
            return Enum.GetNames(typeof(T)).ToList();
        }


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