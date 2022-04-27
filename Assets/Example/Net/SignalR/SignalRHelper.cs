using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;
using WithWhat.Net.SignalR;

public class SignalRHelper : MonoBehaviour
{
    private string msg;

    private void Awake()
    {
        // 初始化客户端
        SignalRClient.Instance.InitClient("http://10.100.100.116:8081/chathub");
        // 注册连接成功回调
        SignalRClient.Instance.OnConnected += OnConnected;
        // 注册连接失败回调
        SignalRClient.Instance.OnDisConnected += OnDisConnected;
        // 注册重连回调
        SignalRClient.Instance.OnReconnecting += OnReconnecting;
    }

    private void Start()
    {
        // 连接服务器
        SignalRClient.Instance.ConnectToServer();
        // 注册远程回调（接收消息）
        SignalRClient.Instance.Connection.On<string, string, string>("ReceiveMessage", (message, user, type) =>
        {
            msg += $"{message} {user} {type}\r\n";
            Debug.Log($"{message} {user} {type}");
        });
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 300, 1000), msg,new GUIStyle() { fontSize=20,fontStyle=FontStyle.Bold});
    }

    private void OnReconnecting()
    {
        print("重新连接");
    }

    private void OnConnected()
    {
        print("已连接服务器");
        msg += "已连接服务器\r\n";
        // 调用远程方法
        SignalRClient.Instance.InvokeAsync("AddToGroup", "topic-2");
    }

    private void OnDisConnected()
    {
        print("已断开连接");
    }
}
