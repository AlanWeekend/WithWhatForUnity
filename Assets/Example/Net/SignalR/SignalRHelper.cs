using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;
using WithWhat.Net.SignalR;

public class SignalRHelper : MonoBehaviour
{
    private string msg;

    private void Awake()
    {
        // ��ʼ���ͻ���
        SignalRClient.Instance.InitClient("http://10.100.100.116:8081/chathub");
        // ע�����ӳɹ��ص�
        SignalRClient.Instance.OnConnected += OnConnected;
        // ע������ʧ�ܻص�
        SignalRClient.Instance.OnDisConnected += OnDisConnected;
        // ע�������ص�
        SignalRClient.Instance.OnReconnecting += OnReconnecting;
    }

    private void Start()
    {
        // ���ӷ�����
        SignalRClient.Instance.ConnectToServer();
        // ע��Զ�̻ص���������Ϣ��
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
        print("��������");
    }

    private void OnConnected()
    {
        print("�����ӷ�����");
        msg += "�����ӷ�����\r\n";
        // ����Զ�̷���
        SignalRClient.Instance.InvokeAsync("AddToGroup", "topic-2");
    }

    private void OnDisConnected()
    {
        print("�ѶϿ�����");
    }
}
