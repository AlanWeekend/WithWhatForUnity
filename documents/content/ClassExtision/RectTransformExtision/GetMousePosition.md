---
title: "GetMousePosition()"
date: 2021-11-22T17:05:46+08:00
draft: false
---

[Github](https://github.com/AlanWeekend/WithWhatForUnity/blob/upm/Runtime/ClassExtision/RectTransformExtision.cs#L13)
[Gitee](https://gitee.com/week233/with_what_for_unity/blob/upm/Runtime/ClassExtision/RectTransformExtision.cs#L13)

WithWhat.ClassExtision
## 描述
获取鼠标在UI上的位置 by lyb
## 参数
| 参数名 | 类型 | 描述 |
| - | - | - |
| camera | Camera |Screen Space - Camera 模式下需要传入画布指定的相机 |
## 返回值
| 类型 | 描述 |
| - | - |
| Vector3 | 鼠标在UI上的位置 |
## 用法：
```C#
using UnityEngine;
using WithWhat.ClassExtision;

public class GetMousePositionTest : MonoBehaviour
{
    void Update()
    {
        print((this.transform as RectTransform).GetMousePosition());
    }
}
```
## 案例
[Github](https://github.com/AlanWeekend/WithWhatForUnity/tree/master/Assets/Example/ClassExtision/RectTransformExtision/GetMousePosition)
[Gitee](https://gitee.com/week233/with_what_for_unity/tree/master/Assets/Example/ClassExtision/RectTransformExtision/GetMousePosition)