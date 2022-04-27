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