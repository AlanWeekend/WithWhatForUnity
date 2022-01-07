---
title: "FindChildByName()"
date: 2021-11-22T16:22:17+08:00
draft: false
---

[Github](https://github.com/AlanWeekend/WithWhatForUnity/blob/upm/Runtime/ClassExtision/TransformExtision.cs#L58)
[Gitee](https://gitee.com/week233/with_what_for_unity/blob/upm/Runtime/ClassExtision/TransformExtision.cs#L58)

WithWhat.ClassExtision
## 描述
根据名称查找子物体（递归）
## 参数
| 参数名 | 类型 | 描述 |
| - | - | - |
| name | string | 物体名称 |
## 用法：
```C#
using UnityEngine;
using WithWhat.ClassExtision;

public class FindChildByNameTest : MonoBehaviour
{
    void Start()
    {
        print(this.transform.FindChildByName("GameObject").name);      
    }
}
```
## 案例
[Github](https://github.com/AlanWeekend/WithWhatForUnity/tree/master/Assets/Example/ClassExtision/TransformExtision/FindChildByName)
[Gitee](https://gitee.com/week233/with_what_for_unity/tree/master/Assets/Example/ClassExtision/TransformExtision/FindChildByName)