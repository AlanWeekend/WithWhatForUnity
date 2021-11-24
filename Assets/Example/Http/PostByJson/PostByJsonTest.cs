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
