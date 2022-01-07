---
title: "Mediator"
date: 2021-11-24T15:10:21+08:00
draft: false
---

[Github](https://github.com/AlanWeekend/WithWhatForUnity/tree/upm/Runtime/DesignPattern/Mediator)
[Gitee](https://gitee.com/week233/with_what_for_unity/tree/upm/Runtime/DesignPattern/Mediator)

WithWhat.DesignPattern
## 描述
中介模式
## 用法
```C#
using UnityEngine;
using WithWhat.DesignPattern;

public class MediatorTest : MonoBehaviour
{
    public enum MediatorTestEnum
    { 
        Test,
    }

    private void Awake()
    {
        Mediator.Instance.Register(MediatorTestEnum.Test, func);       
    }

    private void func(int key, object[] param)
    {
        print($"{key} {param[0] as string}");
    }
}
```
```C#
using UnityEngine;
using WithWhat.DesignPattern;

public class Send : MonoBehaviour
{
    void Start()
    {
        Mediator.Instance.Send(MediatorTest.MediatorTestEnum.Test, "hello");       
    }
}
```