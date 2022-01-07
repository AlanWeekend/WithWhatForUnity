---
title: "SingleTonObejctFactory"
date: 2021-11-24T11:25:04+08:00
draft: false
---

[Github](https://github.com/AlanWeekend/WithWhatForUnity/blob/upm/Runtime/DesignPattern/Factory/SingleTonObejctFactory.cs)
[Gitee](https://gitee.com/week233/with_what_for_unity/blob/upm/Runtime/DesignPattern/Factory/SingleTonObejctFactory.cs)

WithWhat.DesignPattern
## 描述
单例池工厂
## 用法
```C#
using UnityEngine;
using WithWhat.DesignPattern;

public class SingleTonFactoryTest : MonoBehaviour
{
    private SingleTonObejctFactory _singleTonObejctFactory;

    public class HttpService
    {
        public void GetData() { print($"{this.GetHashCode()}:GetData"); }
    }

    private void Awake()
    {
        _singleTonObejctFactory = new SingleTonObejctFactory();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var httpService = _singleTonObejctFactory.AcquireObject<HttpService>();
            httpService.GetData();
        }
    }
}
```