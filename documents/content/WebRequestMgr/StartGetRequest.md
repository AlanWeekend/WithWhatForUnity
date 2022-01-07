---
title: "StartGetRequest()"
date: 2021-11-19T17:12:03+08:00
draft: false
---


[Github](https://github.com/AlanWeekend/WithWhatForUnity/blob/upm/Runtime/Net/Http/WebRequestMgr.cs#L30)
[Gitee](https://gitee.com/week233/with_what_for_unity/blob/upm/Runtime/Net/Http/WebRequestMgr.cs#L30)

WithWhat.Net.Http
## 描述
Get请求
## 参数
| 参数名 | 类型 | 描述 |
| - | - | - |
| url | string | 请求地址,不包含域名 |
| resultFunc | Action<bool, string> | 请求完成后的回调,参数1:是否请求成功,参数2:成功时是json,失败时是错误提示 |
| header | Dictionary<string, string> | 请求头（可选） | 
## 用法：
```C#
using UnityEngine;
using WithWhat.Net.Http;

public class GetTest : MonoBehaviour
{
    private void Awake()
    {
        WebRequestMgr.Instance.Domain = "https://api.muxiaoguo.cn/api/";
    }

    void Start()
    {
        WebRequestMgr.Instance.StartGetRequest("chePhone?phoneNum=13786310544", (success, message) =>
        {
            if (success)
            {
                print(message);
            }
        });        
    }
}
```