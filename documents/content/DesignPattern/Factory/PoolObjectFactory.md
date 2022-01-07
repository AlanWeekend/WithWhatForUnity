---
title: "PoolObjectFactory"
date: 2021-11-24T11:19:45+08:00
draft: false
---

[Github](https://github.com/AlanWeekend/WithWhatForUnity/blob/upm/Runtime/DesignPattern/Factory/PoolObjectFactory.cs)
[Gitee](https://gitee.com/week233/with_what_for_unity/blob/upm/Runtime/DesignPattern/Factory/PoolObjectFactory.cs)

WithWhat.DesignPattern
## 描述
对象池工厂
## 用法
```C#
using UnityEngine;
using WithWhat.DesignPattern;

public class PoolFactoryTest : MonoBehaviour
{
    private PoolObjectFactory _poolObjectFactory;

    public class DBConnect {
        public void Insert() { print($"insert{this.GetHashCode()}"); }
    }

    private void Awake()
    {
        _poolObjectFactory = new PoolObjectFactory(100, false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 从对象池获取对象
            var dbConnect = _poolObjectFactory.AcquireObject<DBConnect>();
            dbConnect.Insert();
            // 使用完，返还给对象池
            _poolObjectFactory.ReleaseObject(dbConnect);
        }
    }
}
```