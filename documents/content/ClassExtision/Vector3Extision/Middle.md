---
title: "Middle()"
date: 2021-11-22T16:30:03+08:00
draft: false
---

[Github](https://github.com/AlanWeekend/WithWhatForUnity/blob/upm/Runtime/ClassExtision/VectorExtision.cs#L13)
[Gitee](https://gitee.com/week233/with_what_for_unity/blob/upm/Runtime/ClassExtision/VectorExtision.cs#L13)

WithWhat.ClassExtision
## 描述
计算两个点的中点
## 参数
| 参数名 | 类型 | 描述 |
| - | - | - |
| to | Vector3 | 另一个点 |
## 返回值
| 类型 | 描述 |
| - | - |
| Vector3 | 中点 |
## 用法：
```C#
using UnityEngine;
using WithWhat.ClassExtision;

public class MiddleTest : MonoBehaviour
{
    public GameObject other;

    void Start()
    {
        print(this.transform.position.Middle(other.transform.position));
    }
}
```
## 案例
[Github](https://github.com/AlanWeekend/WithWhatForUnity/tree/master/Assets/Example/ClassExtision/Vector3Extision/Middle)
[Gitee](https://gitee.com/week233/with_what_for_unity/tree/master/Assets/Example/ClassExtision/Vector3Extision/Middle)