---
title: "SetPostionToMousePosition()"
date: 2021-11-22T16:06:00+08:00
draft: false
---

[Github](https://github.com/AlanWeekend/WithWhatForUnity/blob/upm/Runtime/ClassExtision/TransformExtision.cs#L43)
[Gitee](https://gitee.com/week233/with_what_for_unity/blob/upm/Runtime/ClassExtision/TransformExtision.cs#L43)

WithWhat.ClassExtision
## 描述
设置物体的世界坐标为鼠标所在位置
## 参数
| 参数名 | 类型 | 描述 |
| - | - | - |
| camera | Camera | 相机 |
| targetGameObjectPosition | Vector3 | 目标物体(参考物)坐标 |
| offset | Vector3 | 偏移量 |
## 用法：
```C#
using UnityEngine;
using WithWhat.ClassExtision;

public class SetTheWorldPostionToTheMousePositionTest : MonoBehaviour
{
    public GameObject plan;

    void Update()
    {
        this.transform.SetPostionToMousePosition(Camera.main, plan.transform.position, Vector3.zero);
    }
}
```
## 案例
[Github](https://github.com/AlanWeekend/WithWhatForUnity/tree/master/Assets/Example/ClassExtision/TransformExtision/SetTheWorldPostionToTheMousePosition)
[Gitee](https://gitee.com/week233/with_what_for_unity/tree/master/Assets/Example/ClassExtision/TransformExtision/SetTheWorldPostionToTheMousePosition)