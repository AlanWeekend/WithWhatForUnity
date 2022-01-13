using System;
using System.Collections.Generic;


namespace WithWhat.Utils
{
    public static class ListUtils
    {
        /// <summary>
        /// 删除集合中符合条件的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <param name="match"></param>
        public static void RemoveMatch<T>(this List<T> lst, Predicate<T> match)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                while (i < lst.Count && match(lst[i]))
                {
                    lst.RemoveAt(i);
                }
            }
        }
    }
}