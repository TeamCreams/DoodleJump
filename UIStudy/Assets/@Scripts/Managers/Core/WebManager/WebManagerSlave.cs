﻿using System;
using System.Collections;
using System.Net.Http;
using System.Security.Policy;
using Unity.VisualScripting;
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

            string[] pages = url.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    callback?.Invoke(webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    IEnumerator SendPostRequestCo(string url, string body, Action<string> callback = null)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, body, "application/json"))
        {
            // Request and wait for the desired page.
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                callback?.Invoke(www.downloadHandler.text);
            }
        }
    }
}