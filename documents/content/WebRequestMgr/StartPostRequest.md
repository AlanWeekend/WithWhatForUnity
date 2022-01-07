---
title: "StartPostRequest()"
date: 2021-11-23T11:54:30+08:00
draft: false
---

[Github](https://github.com/AlanWeekend/WithWhatForUnity/blob/upm/Runtime/Net/Http/WebRequestMgr.cs#L44)
[Gitee](https://gitee.com/week233/with_what_for_unity/blob/upm/Runtime/Net/Http/WebRequestMgr.cs#L44)

WithWhat.Net.Http
## 描述
Post请求
## 参数
| 参数名 | 类型 | 描述 |
| - | - | - |
| url | string | 请求地址,不包含域名 |
| postData | WWWForm | 表单 |
| resultFunc | Action<bool, string> | 请求完成后的回调,参数1:是否请求成功,参数2:成功时是json,失败时是错误提示 |
| header | Dictionary<string, string> | 请求头（可选） | 
## 用法：
```C#
using UnityEngine;
using WithWhat.Net.Http;

public class PostTest : MonoBehaviour
{
    private void Awake()
    {
        WebRequestMgr.Instance.Domain = "https://api.muxiaoguo.cn/api/";
    }

    void Start()
    {
        var requestData = new WWWForm();
        requestData.AddField("phoneNum", "13786310544");
        WebRequestMgr.Instance.StartPostRequest("chePhone", requestData, (success, message) =>
        {
            if (success)
            {
                print(message);
            }
        });
    }
}
```