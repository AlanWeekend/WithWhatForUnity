using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;
using WithWhat.Net.SignalR;

public class SignalRHelper : MonoBehaviour
{
    private string msg;

    private void Awake()
    {
        SignalRClient.Instance.InitClient("http://10.100.100.116:8081/chathub");
        SignalRClient.Instance.OnConnected += OnConnected;
        SignalRClient.Instance.OnDisConnected += OnDisConnected;
        SignalRClient.Instance.OnReconnecting += OnReconnecting;
    }

    private void Start()
    {
        SignalRClient.Instance.ConnectToServer();
        SignalRClient.Instance.Connection.On<string, string, string>("ReceiveMessage", (message, user, type) =>
        {
            msg += $"{message} {user} {type}\r\n";
            Debug.Log($"{message} {user} {type}");
        });
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 300, 1000), msg,new GUIStyle() { fontSize=20,fontStyle=FontStyle.Bold});//每帧绘制的,如果isshow=false,则不显示
    }

    private void OnReconnecting()
    {
        print("重新连接");
    }

    private void OnConnected()
    {
        print("已连接服务器");
        msg += "已连接服务器\r\n";
        SignalRClient.Instance.InvokeAsync("AddToGroup", "topic-2");
    }

    private void OnDisConnected()
    {
        print("已断开连接");
    }
}
