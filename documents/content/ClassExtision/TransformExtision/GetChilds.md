---
title: "GetChilds()"
date: 2021-11-22T15:47:44+08:00
draft: false
---

[Github](https://github.com/AlanWeekend/WithWhatForUnity/blob/upm/Runtime/ClassExtision/TransformExtision.cs#L13)
[Gitee](https://gitee.com/week233/with_what_for_unity/blob/upm/Runtime/ClassExtision/TransformExtision.cs#L13)

WithWhat.ClassExtision
## 描述
获取所有的子物体，不包含孙物体
## 返回值
| 类型 | 描述 |
| - | - |
| List<Transform> | 子物体集合 |
## 用法：
```C#
using UnityEngine;
using WithWhat.ClassExtision;

public class GetChildsTest : MonoBehaviour
{
    void Start()
    {
        foreach (var child in this.transform.GetChilds())
        {
            print(child.name);
        }
    }
}
```
## 案例
[Github](https://github.com/AlanWeekend/WithWhatForUnity/tree/master/Assets/Example/ClassExtision/TransformExtision/GetChilds)
[Gitee](https://gitee.com/week233/with_what_for_unity/tree/master/Assets/Example/ClassExtision/TransformExtision/GetChilds)