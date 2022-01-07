---
title: "SetChildsActive()"
date: 2021-11-22T15:53:44+08:00
draft: false
---

[Github](https://github.com/AlanWeekend/WithWhatForUnity/blob/upm/Runtime/ClassExtision/TransformExtision.cs#L27)
[Gitee](https://gitee.com/week233/with_what_for_unity/blob/upm/Runtime/ClassExtision/TransformExtision.cs#L27)

WithWhat.ClassExtision
## 描述
设置所有子物体的active，不包含孙物体
## 参数
| 参数名 | 类型 | 描述 |
| - | - | - |
| value | bool | 显示or隐藏 |
## 用法：
```C#
using UnityEngine;
using WithWhat.ClassExtision;

public class SetChildsActiveTest : MonoBehaviour
{
    private void Start()
    {
        this.transform.SetChildsActive(false);
    }
}
```
## 案例
[Github]https://github.com/AlanWeekend/WithWhatForUnity/tree/master/Assets/Example/ClassExtision/TransformExtision/SetChildsActive)
[Gitee](https://gitee.com/week233/with_what_for_unity/tree/master/Assets/Example/ClassExtision/TransformExtision/SetChildsActive)