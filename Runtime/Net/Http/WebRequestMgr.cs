using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using ZCCUtils.DesignPattern;

namespace ZCCUtils.Net.Http
{
    public class WebRequestMgr : MonoSingleTon<WebRequestMgr>
    {
        /// <summary>
        /// 域名
        /// </summary>
        public string Domain;
        /// <summary>
        /// 请求头
        /// </summary>
        public Dictionary<string, string> Header;

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url">请求地址,不包含域名</param>
        /// <param name="loop">循环请求</param>
        /// <param name="resultFunc">请求完成后的回调,参数1:是否请求成功,参数2:成功时是json,失败时是错误提示</param>
        /// <returns></returns>
        public void StartGetRequest(string url, Action<bool, string> resultFunc, Dictionary<string, string> header = null)
        {
            var complateUrl = $"{Domain}{url}";
            StartCoroutine(Get(complateUrl, resultFunc, header));
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">请求地址,不包含域名</param>
        /// <param name="postData">表单</param>
        /// <param name="loop">循环请求</param>
        /// <param name="resultFunc">请求完成后的回调,参数1:是否请求成功,参数2:成功时是json,失败时是错误提示</param>
        /// <returns></returns>
        public void StartPostRequest(string url, WWWForm postData, Action<bool, string> resultFunc, Dictionary<string, string> header = null)
        {
            var complateUrl = $"{Domain}{url}";
            StartCoroutine(Post(complateUrl, postData, resultFunc, header));
        }

        /// <summary>
        /// Post请求，发送一个json
        /// </summary>
        /// <param name="url">请求地址,不包含域名</param>
        /// <param name="postData">json</param>
        /// <param name="loop">循环请求</param>
        /// <param name="resultFunc">请求完成后的回调,参数1:是否请求成功,参数2:成功时是json,失败时是错误提示</param>
        /// <returns></returns>
        public void StartPostRequestByJson(string url, string postData, Action<bool, string> resultFunc, Dictionary<string, string> header = null)
        {
            var complateUrl = $"{Domain}{url}";
            StartCoroutine(PostByJson(complateUrl, postData, resultFunc, header));
        }

        /// <summary>
        /// Get方法请求接口
        /// </summary>
        /// <param name="url">请求地址,不包含域名</param>
        /// <param name="resultFunc">请求完成后的回调,参数1:是否请求成功,参数2:成功时是json,失败时是错误提示</param>
        /// <returns></returns>
        private IEnumerator Get(string url, Action<bool, string> resultFunc, Dictionary<string, string> header)
        {
            string decodeUrl = DecodeURLParam(url);
            using (UnityWebRequest www = UnityWebRequest.Get(decodeUrl))
            {
                SetHeader(www, header);
                yield return www.SendWebRequest();
                ResultCheck(www, resultFunc);
                www.Dispose();
            }
        }

        /// <summary>
        /// Post方法请求接口
        /// </summary>
        /// <param name="url">请求地址,不包含域名</param>
        /// <param name="postData">参数</param>
        /// <param name="resultFunc">请求完成后的回调,参数1:是否请求成功,参数2:成功时是json,失败时是错误提示</param>
        /// <returns></returns>
        private IEnumerator Post(string url, WWWForm postData, Action<bool, string> resultFunc, Dictionary<string, string> header)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(url, postData))
            {
                SetHeader(www,header);
                yield return www.SendWebRequest();
                ResultCheck(www, resultFunc);
                www.Dispose();
            }
        }

        /// <summary>
        /// Post方法请求接口，发送json
        /// </summary>
        /// <param name="url">请求地址,不包含域名</param>
        /// <param name="postData">json</param>
        /// <param name="resultFunc">请求完成后的回调,参数1:是否请求成功,参数2:成功时是json,失败时是错误提示</param>
        /// <returns></returns>
        private IEnumerator PostByJson(string url, string postData, Action<bool, string> resultFunc, Dictionary<string, string> header)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(url, postData))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(postData);
                www.uploadHandler = new UploadHandlerRaw(bodyRaw);
                www.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
                SetHeader(www,header);
                yield return www.SendWebRequest();
                ResultCheck(www, resultFunc);
                // 手动清理www资源
                www.Dispose();
            }
        }

        /// <summary>
        /// 设置请求头
        /// </summary>
        /// <param name="www">请求对象</param>
        /// <param name="header">请求头 key value</param>
        private void SetHeader(UnityWebRequest www, Dictionary<string, string> header)
        {
            var hdr = header == null ? this.Header : header;
            if (hdr == null) return;
            foreach (var key in hdr.Keys)
            {
                www.SetRequestHeader(key, hdr[key]);
            }
        }

        /// <summary>
        /// 验证请求结果
        /// </summary>
        /// <param name="www"></param>
        /// <param name="resultFunc"></param>
        private void ResultCheck(UnityWebRequest www, Action<bool, string> resultFunc)
        {
            if (www.isNetworkError || www.isHttpError)
            {
                resultFunc(false, www.url + " | " + www.error);
            }
            else
            {
                resultFunc(true, www.downloadHandler.text);
            }
        }

        /// <summary>
        /// 编码URL中的参数
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string DecodeURLParam(string url)
        {
            var decodeUrl = url;

            // 正则匹配参数
            var parms = new List<string>();
            string pattern = @"[\?&]([a-zA-Z_]+)=([\w_.#]*)";
            foreach (Match match in Regex.Matches(url, pattern))
                parms.Add(match.Value);

            // 替换编码后的参数
            foreach (var parm in parms)
            {
                var parmValue = parm.Substring(parm.IndexOf('=') + 1);
                var decodeParmValue = Uri.EscapeDataString(parmValue);
                // 空参剔除，有的接口需要传空参
                if (!string.IsNullOrEmpty(parmValue))
                    decodeUrl = decodeUrl.Replace(parmValue, decodeParmValue);
            }

            return decodeUrl;
        }
    }
}