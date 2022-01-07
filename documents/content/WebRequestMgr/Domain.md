---
title: "Domain"
date: 2021-11-19T17:12:03+08:00
draft: false
---

[Github](https://github.com/AlanWeekend/WithWhatForUnity/blob/upm/Runtime/Net/Http/WebRequestMgr.cs#L17)
[Gitee](https://gitee.com/week233/with_what_for_unity/blob/upm/Runtime/Net/Http/WebRequestMgr.cs#L17)

WithWhat.Net.Http
## 描述
设置Http请求的基础域名，必须设置
## 用法
```C#
using UnityEngine;
using WithWhat.Net.Http;

public class DomainTest : MonoBehaviour
{
    private void Awake()
    {
        WebRequestMgr.Instance.Domain = "https://api.muxiaoguo.cn/api/chePhone/";
    }
}
```