---
title: "WorldToCanvasPoint()"
date: 2021-11-22T15:14:42+08:00
draft: false
---

[Github](https://github.com/AlanWeekend/WithWhatForUnity/blob/upm/Runtime/ClassExtision/RectTransformExtision.cs#L26)
[Gitee](https://gitee.com/week233/with_what_for_unity/blob/upm/Runtime/ClassExtision/RectTransformExtision.cs#L26)

WithWhat.ClassExtision
## 描述
世界坐标转ui坐标
## 参数
| 参数名 | 类型 | 描述 |
| - | - | - |
| canvas |Canvas | 画布 |
| worldPos |Vector3 | 世界坐标 |
## 返回值
| 类型 | 描述 |
| - | - |
| Vector2 | UI坐标 |
## 用法：
```C#
using UnityEngine;
using WithWhat.ClassExtision;

public class WorldToCanvasPointTest : MonoBehaviour
{
    public GameObject Cube;
    public Canvas canvas;

    void Start()
    {
        print(RectTransformExtision.WorldToCanvasPoint(canvas, Cube.transform.position));
    }
}
```
## 案例
[Github](https://github.com/AlanWeekend/WithWhatForUnity/tree/master/Assets/Example/ClassExtision/RectTransformExtision/WorldToCanvasPoint)
[Gitee](https://gitee.com/week233/with_what_for_unity/tree/master/Assets/Example/ClassExtision/RectTransformExtision/WorldToCanvasPoint)