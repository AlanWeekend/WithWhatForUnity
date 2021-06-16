﻿using System.Collections.Generic;
using UnityEngine;

namespace ZCCUtils.Utils
{
    public struct MathUtils
    {
        /// <summary>
        /// 计算二次贝塞尔曲线点在某个采样上的点
        /// </summary>
        /// <param name="t">采样</param>
        /// <param name="p0">起点</param>
        /// <param name="p1">控制点</param>
        /// <param name="p2">终点</param>
        /// <returns>二次贝塞尔曲线一个采样的点</returns>
        public static Vector3 Bezier_2_Single(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            return (1 - t) * ((1 - t) * p0 + t * p1) + t * ((1 - t) * p1 + t * p2);
        }

        /// <summary>
        /// 计算二次贝塞尔曲线上的所有点
        /// </summary>
        /// <param name="startPoint">起点</param>
        /// <param name="endPoint">终点</param>
        /// <param name="controlPoint">控制点</param>
        /// <param name="segmentNum">采样数</param>
        /// <returns>二次贝塞尔曲线上的所有点，点个数取决于采样率</returns>
        public static List<Vector3> Bezier_2(Vector3 startPoint, Vector3 endPoint, Vector3 controlPoint, int segmentNum = 10)
        {
            var points = new List<Vector3>();
            for (int i = 1; i < segmentNum; i++)
            {
                float t = i / (float)segmentNum;
                Vector3 pixel = Bezier_2_Single(t, startPoint, controlPoint, endPoint);
                points.Add(pixel);
            }
            return points;
        }
    }
}