---
title: "StartPostRequestByJson()"
date: 2021-11-23T12:01:14+08:00
draft: false
---

[Github](https://github.com/AlanWeekend/WithWhatForUnity/blob/upm/Runtime/Net/Http/WebRequestMgr.cs#L58)
[Gitee](https://gitee.com/week233/with_what_for_unity/blob/upm/Runtime/Net/Http/WebRequestMgr.cs#L58)

WithWhat.Net.Http
## 描述
Post请求，通过json传参
## 参数
| 参数名 | 类型 | 描述 |
| - | - | - |
| url | string | 请求地址,不包含域名 |
| postData | string | Json数据 |
| resultFunc | Action<bool, string> | 请求完成后的回调,参数1:是否请求成功,参数2:成功时是json,失败时是错误提示 |
| header | Dictionary<string, string> | 请求头（可选） | 
## 用法：
```C#
using UnityEngine;
using WithWhat.Net.Http;
using Newtonsoft.Json;

public class PostByJsonTest : MonoBehaviour
{
    class Payload
    {
        public string url;
        public string type;
    }

    private void Awake()
    {
        WebRequestMgr.Instance.Domain = "https://api.muxiaoguo.cn/api/";
    }

    void Start()
    {
        var payload = new Payload() { url = "qq.com",type= "Cz" };
        WebRequestMgr.Instance.StartPostRequestByJson("ICP", JsonConvert.SerializeObject(payload), (success, message) =>
        {
            if (success)
            {
                print(message);
            }
        });
    }
}

```