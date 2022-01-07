---
title: "TransientObjectFactory"
date: 2021-11-24T11:46:06+08:00
draft: false
---

[Github](https://github.com/AlanWeekend/WithWhatForUnity/blob/upm/Runtime/DesignPattern/Factory/TransientObjectFactory.cs)
[Gitee](https://gitee.com/week233/with_what_for_unity/blob/upm/Runtime/DesignPattern/Factory/TransientObjectFactory.cs)

WithWhat.DesignPattern
## 描述
临时对象池工厂
## 用法
```C#
using UnityEngine;
using WithWhat.DesignPattern;

public class TransientFactoryTest : MonoBehaviour
{
    private TransientObjectFactory _transientObjectFactory;

    public class Foo 
    {
        public void Do() { print($"{GetHashCode()}:DoSomething"); }
    }

    private void Awake()
    {
        _transientObjectFactory = new TransientObjectFactory();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var foo = _transientObjectFactory.AcquireObject<Foo>();
            foo.Do();
        }
    }
}
```