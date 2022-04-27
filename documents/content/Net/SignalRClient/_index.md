---
title: "SignalRClient"
date: 2021-11-22T13:59:12+08:00
draft: false
---

[Github](https://github.com/AlanWeekend/WithWhatForUnity/blob/upm/Runtime/Net/SignalR/SignalRClient.cs)
[Gitee](https://gitee.com/week233/with_what_for_unity/blob/upm/Runtime/Net/SignalR/SignalRClient.cs)

WithWhat.Net.SignalR
## 描述
SignalR客户端，仅支持``.NET4.X``。``.NET Standard 2.0``需要自行替换``Runtime\Net\SignalR\Plugins``中的DLL。
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