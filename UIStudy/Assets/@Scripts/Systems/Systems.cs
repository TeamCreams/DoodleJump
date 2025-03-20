using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Systems : MonoBehaviour
{

	private static Systems s_instance;
	private static Systems Instance { get { Init(); return s_instance; } }

	#region Contents
	private UserSystem _user = new UserSystem();
	private GoogleLoginWebViewSystem _googleLoginWebView = new GoogleLoginWebViewSystem();

    public static UserSystem User { get { return Instance?._user; } }
    public static GoogleLoginWebViewSystem GoogleLoginWebView { get { return Instance?._googleLoginWebView; } }
    #endregion


    public static void Init()
	{
		if (s_instance == null)
		{
			GameObject go = GameObject.Find("@Systems");
			if (go == null)
			{
				go = new GameObject { name = "@Systems" };
				go.AddComponent<Systems>();
			}

			DontDestroyOnLoad(go);

			// 초기화
			s_instance = go.GetComponent<Systems>();
		}
	}
}
