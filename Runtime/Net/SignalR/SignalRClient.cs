using Microsoft.AspNetCore.SignalR.Client;
using System;
using WithWhat.DesignPattern;
using UnityEngine;

namespace WithWhat.Net.SignalR
{
    public class SignalRClient : MonoSingleTon<SignalRClient>
    {
        private HubConnection connection;
        private HubConnectionState lastHubConnectionState;
        private string url;
        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool IsConnected { get { return connection != null && connection.State == HubConnectionState.Connected; } }
        /// <summary>
        /// 连接对象
        /// </summary>
        public HubConnection Connection { get { return connection; } set { connection = value; } }
        /// <summary>
        /// 连接服务器成功回调
        /// </summary>
        public Action OnConnected;
        /// <summary>
        /// 连接服务器失败回调
        /// </summary>
        public Action OnDisConnected;
        /// <summary>
        /// 服务器重连回调
        /// </summary>
        public Action OnReconnecting;

        #region Unity Event Funcionts
        private void Update()
        {
            // 连接成功
            if (lastHubConnectionState == HubConnectionState.Connecting && IsConnected)
            {
                OnConnected?.Invoke();
            }
            // 连接失败
            if (lastHubConnectionState == HubConnectionState.Connecting && connection.State == HubConnectionState.Disconnected)
            {
                OnConnected?.Invoke();
            }
            // 重连
            if (lastHubConnectionState == HubConnectionState.Connecting && connection.State == HubConnectionState.Reconnecting)
            {
                OnReconnecting?.Invoke();
            }
            lastHubConnectionState = connection.State;
        }

        private void OnDisable()
        {
            connection.StopAsync();
        }
        #endregion

        #region Public Functions
        public void InitClient(string url)
        {
            this.url = url;
            connection = new HubConnectionBuilder()
                .WithUrl(this.url)
                .WithAutomaticReconnect()
                .Build();
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        public void ConnectToServer(string url="")
        {
            if (!IsConnected)
            {
                if (!string.IsNullOrEmpty(url)) this.url = url;
                connection = new HubConnectionBuilder()
                    .WithUrl(this.url)
                    .WithAutomaticReconnect()
                    .Build();
            }
            
            // 开始连接
            try
            {
                connection.StartAsync();
                lastHubConnectionState = connection.State;
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }

        /// <summary>
        /// 调用远程方法
        /// </summary>
        /// <param name="method"></param>
        /// <param name="param"></param>
        public void InvokeAsync(string method,string param) {
            if (IsConnected)
                connection.InvokeAsync(method, param);
        }
        #endregion
    }
}