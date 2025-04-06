using System;
using System.Collections;
using System.Net.Http;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebManagerSlave : InitBase
{
    public override bool Init()
    {
        if (false == base.Init())
        {
            return false;
        }

        DontDestroyOnLoad(this.gameObject);

        return true;
    }


    public void SendGetRequest(string url, Action<string> callback = null)
    {
        StartCoroutine(SendGetRequestCo(url, callback));
    }

    public void SendPostRequest(string url, string body, Action<string> callback = null)
    {
        StartCoroutine(SendPostRequestCo(url, body, callback));
    }


    IEnumerator SendGetRequestCo(string url, Action<string> callback = null)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<color=red><b>Web Log End : </b></color>");
            builder.AppendLine($" url : {url}");

            string[] pages = url.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    builder.AppendLine($" {pages[page]} : Error: {webRequest.error}");
                    callback?.Invoke(null);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    builder.AppendLine($" {pages[page]} : HTTP Error: {webRequest.error}");
                    callback?.Invoke(null);
                    break;
                case UnityWebRequest.Result.Success:
                    callback?.Invoke(webRequest.downloadHandler.text);
                    break;
            }
            Debug.Log(builder.ToString());
        }
    }

    IEnumerator SendPostRequestCo(string url, string body, Action<string> callback = null)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, body, "application/json"))
        {
            // Request and wait for the desired page.
            yield return www.SendWebRequest();

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<color=red><b>Web Log End : </b></color>");
            builder.AppendLine($" url : {url}");
            builder.AppendLine($" body : {body}");

            if (www.result != UnityWebRequest.Result.Success)
            {
                builder.AppendLine($" error : {www.error}");
                callback?.Invoke(null);
            }
            else
            {
                builder.AppendLine($" response : {www.downloadHandler.text}");
                callback?.Invoke(www.downloadHandler.text);
            }
            Debug.Log(builder.ToString());
        }
    }
}