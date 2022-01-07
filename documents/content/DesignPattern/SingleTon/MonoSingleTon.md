---
title: "MonoSingleTon"
date: 2021-11-23T14:02:09+08:00
draft: false
---

[Github](https://github.com/AlanWeekend/WithWhatForUnity/blob/upm/Runtime/DesignPattern/SingleTon/MonoSingleTon.cs)
[Gitee](https://gitee.com/week233/with_what_for_unity/blob/upm/Runtime/DesignPattern/SingleTon/MonoSingleTon.cs)

WithWhat.DesignPattern
## 描述
Mono单例模板，当脚本没有手动挂载到GameObject上时，会自动创建一个DontDestory的物体
## 用法
```C#
using UnityEngine;
using WithWhat.DesignPattern;

public class Foo : MonoSingleTon<Foo>
{
    public void DoSomething()
    {
        print("do it");
    }
}

public class MonoSingleTonTest : MonoBehaviour
{
    void Start()
    {
        Foo.Instance.DoSomething();       
    }
}
```