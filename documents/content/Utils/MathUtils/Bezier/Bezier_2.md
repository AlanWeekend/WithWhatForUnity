---
title: "Bezier_2()"
date: 2021-11-24T15:32:38+08:00
draft: false
---

[Github](https://github.com/AlanWeekend/WithWhatForUnity/blob/upm/Runtime/Utils/MathUtils.cs#L29)
[Gitee](https://gitee.com/week233/with_what_for_unity/blob/upm/Runtime/Utils/MathUtils.cs#L29)

WithWhat.Utils
## 描述
计算二次贝塞尔曲线上的所有点
## 参数
| 参数名 | 类型 | 描述 |
| - | - | - |
| startPoint | Vector3 | 起点 |
| endPoint | Vector3 | 终点 |
| conttolPoint | Vector3 | 控制点 |
| segementNum | int | 采样率，默认10 |
## 返回值
| 类型 | 描述 |
| - | - |
| List<Vector3> | 二次贝塞尔曲线上的所有点，点个数取决于采样率 |