---
title: "ServiceLocator"
date: 2021-11-24T14:49:46+08:00
draft: false
---

[Github](https://github.com/AlanWeekend/WithWhatForUnity/tree/upm/Runtime/DesignPattern/Inject)
[Gitee](https://gitee.com/week233/with_what_for_unity/tree/upm/Runtime/DesignPattern/Inject)

WithWhat.DesignPattern
## 描述
服务定位模式，用于注入
## 用法
```C#
using UnityEngine;
using WithWhat.DesignPattern;

public class InjectTest : MonoBehaviour
{
    public class Foo {
        public void Do() { print($"{GetHashCode()}"); }
    }

    private void Awake()
    {
        ServiceLocator.RegisterSingleTon<Foo>();
    }

    void Start()
    {
        var foo = ServiceLocator.Resolve<Foo>();
        foo.Do();
    }
}
```