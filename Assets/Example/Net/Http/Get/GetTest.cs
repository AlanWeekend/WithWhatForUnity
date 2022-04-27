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
