---
title: "GetEnumNamesList()"
date: 2021-11-22T13:59:12+08:00
draft: false
---

[Github](https://github.com/AlanWeekend/WithWhatForUnity/blob/upm/Runtime/ClassExtision/EnumExtision.cs#L14)
[Gitee](https://gitee.com/week233/with_what_for_unity/blob/upm/Runtime/ClassExtision/EnumExtision.cs#L14)

WithWhat.ClassExtision
## 描述
获取枚举类型中所有的字段名组成的字符串List
## 返回值
| 类型 | 描述 |
| - | - |
| List<string> | 枚举字段的字符串 |
## 用法：
```C#
using UnityEngine;
using WithWhat.ClassExtision;

public class Test : MonoBehaviour
{

    enum UIEnum
    {
        Login,
        Register,
    }

    void Start()
    {
        print(typeof(UIEnum).GetEnumNamesList());
    }
}
```