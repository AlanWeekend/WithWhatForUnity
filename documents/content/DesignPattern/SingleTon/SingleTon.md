---
title: "SingleTon"
date: 2021-11-23T14:08:45+08:00
draft: false
---

[Github](https://github.com/AlanWeekend/WithWhatForUnity/blob/upm/Runtime/DesignPattern/SingleTon/SingleTon.cs)
[Gitee](https://gitee.com/week233/with_what_for_unity/blob/upm/Runtime/DesignPattern/SingleTon/SingleTon.cs)

WithWhat.DesignPattern
## 描述
单例模板
## 用法
```C#
using UnityEngine;
using WithWhat.DesignPattern;

public class Foo1 : Singleton<Foo1>
{
    private Foo1() { }

    public void DoSomething()
    {
        Debug.Log("i do");
    }
}

public class SingleTonTest : MonoBehaviour
{
    void Start()
    {
        Foo1.Instance.DoSomething();
    }
}

```