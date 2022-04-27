using UnityEngine;
using WithWhat.Net.Http;

public class DomainTest : MonoBehaviour
{
    private void Awake()
    {
        WebRequestMgr.Instance.Domain = "https://api.muxiaoguo.cn/api/";
    }
}