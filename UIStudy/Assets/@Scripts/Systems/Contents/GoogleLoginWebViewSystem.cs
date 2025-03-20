using Data;
using Gpm.WebView;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
using static Define;
using static Gpm.WebView.GpmWebViewCallback;

public class GoogleLoginWebViewSystem
{
    public void ShowUrl()
    {
        Debug.Log("SSS");
        GpmWebView.ShowUrl(
            "https://www.google.com",
            new GpmWebViewRequest.Configuration()
            {
                style = GpmWebViewStyle.POPUP,
                
            },
            null,
            new List<string>()
            {
            "USER_ CUSTOM_SCHEME"
            });
    }

    private void OnOpenCallback(GpmWebViewError error)
    {
        if (error == null)
        {
            Debug.Log("[OnOpenCallback] succeeded.");
        }
        else
        {
            Debug.Log(string.Format("[OnOpenCallback] failed. error:{0}", error));
        }
    }

    private void OnCloseCallback(GpmWebViewError error)
    {
        if (error == null)
        {
            Debug.Log("[OnCloseCallback] succeeded.");
        }
        else
        {
            Debug.Log(string.Format("[OnCloseCallback] failed. error:{0}", error));
        }
    }

    private void OnSchemeEvent(string data, GpmWebViewError error)
    {
        if (error == null)
        {
            Debug.Log("[OnSchemeEvent] succeeded.");

            if (data.Equals("USER_ CUSTOM_SCHEME") == true || data.Contains("CUSTOM_SCHEME") == true)
            {
                Debug.Log(string.Format("scheme:{0}", data));
            }
        }
        else
        {
            Debug.Log(string.Format("[OnSchemeEvent] failed. error:{0}", error));
        }
    }
}
