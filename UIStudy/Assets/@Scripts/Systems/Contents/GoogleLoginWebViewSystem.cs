using Data;
using Gpm.WebView;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEditor.PackageManager;
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


    private string clientId = "22498118512-mgb6ru1h1m7d5idgm5q4ak4vchret12p.apps.googleusercontent.com";
    //secret key : "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"
    private string scope = "email profile";
    private string redirectUri = "yourapp://oauth2callback";

    // URL 관련 상수
    private const string GOOGLE_AUTH_URL = "https://accounts.google.com/o/oauth2/auth";
    private const string GOOGLE_USER_INFO_URL = "https://www.googleapis.com/oauth2/v2/userinfo";

    // 콜백 이벤트
    public delegate void SignInCallback(GoogleUserInfo userInfo);
    public event SignInCallback OnSignInSuccess;
    public event Action<string> OnSignInFailed;

    public GoogleLoginWebViewSystem()
    {
        // 초기화 로직
        // DeepLink 이벤트 구독
        //DeepLinkManager.OnDeepLinkReceived += OnDeepLinkReceived;

        // 플랫폼에 따라 리디렉트 URI 설정
#if UNITY_EDITOR
        redirectUri = "http://localhost:12000/oauth2callback";
#elif UNITY_ANDROID
        redirectUri = "com.yourcompany.yourapp://oauth2callback";
#elif UNITY_IOS
        redirectUri = "com.yourcompany.yourapp://oauth2callback";
#endif

        OnSignInSuccess += (userInfo) =>
        {
            Debug.Log($"Sign in success: {userInfo.name} ({userInfo.email})");
        };

        OnSignInFailed += (error) =>
        {
            Debug.LogError($"Sign in failed: {error}");
        };

    }

    public void SignIn()
    {

        // 구글 OAuth URL 생성
        string authUrl = $"{GOOGLE_AUTH_URL}" +
                        $"?client_id={clientId}" +
                        $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                        $"&response_type=token" +
                        $"&scope={Uri.EscapeDataString(scope)}";

#if UNITY_EDITOR
        // 에디터에서는 시스템 브라우저 사용
        Application.OpenURL(authUrl);

        // 로컬 웹서버를 시작하여, 리디렉션을 감지
        EditorOAuthListener.StartListening(redirectUri, OnEditorAuthComplete);
#else
        // 모바일에서는 GpmWebView 사용
        GpmWebViewRequest request = new GpmWebViewRequest
        {
            url = authUrl,
            fullScreen = true,
            orientationMask = GpmOrientation.PORTRAIT | GpmOrientation.LANDSCAPE,
            configuration = new GpmWebViewConfiguration
            {
                title = "Google Sign In",
                navigationBarColor = "#4285F4",
                statusBarColor = "#4285F4"
            },
            callback = new GpmWebViewCallback
            {
                onPageStarted = OnPageStarted,
                onPageFinished = OnPageFinished,
                onClose = OnWebViewClosed
            }
        };
        
        GpmWebView.ShowUrl(request);
#endif
    }

#if UNITY_EDITOR
    // 에디터에서 OAuth 완료 처리
    private void OnEditorAuthComplete(string url)
    {
        EditorOAuthListener.StopListening();
        ExtractTokenFromUrl(url);
    }
#else
    private void OnPageStarted(string url)
    {
        Debug.Log($"WebView page started: {url}");
    }

    private void OnPageFinished(string url)
    {
        Debug.Log($"WebView page finished: {url}");
    }

    private void OnWebViewClosed()
    {
        Debug.Log("WebView closed");
    }
#endif


    // 앱으로 들어오는 딥링크 처리 (모바일)
    private void HandleDeepLink(string url)
    {
        Debug.Log($"Deep link received: {url}");

        if (url.StartsWith(redirectUri))
        {
            ExtractTokenFromUrl(url);
        }
    }

    // URL에서 액세스 토큰 추출
    private void ExtractTokenFromUrl(string url)
    {
        try
        {
            Debug.Log($"uri : {url}");
            Uri uri = new Uri(url);
            string fragment = uri.Fragment;

            if (!string.IsNullOrEmpty(fragment) && fragment.StartsWith("#"))
            {
                //http://localhost:12000/oauth2callback#access_token=ya29.a0AeXRPp5TKOivGZADwrhDEmDjoUFtES5V8UGMCKct06Adwsg9_vhE_35lJHhSUl2qUyOYmlckrvta9iwRczWvJCQo3uAbpIm7wE_ubVuXzA-ABqQqMGuMJ1XQa4UdNUp-w-mn52bWDFE5OsSC3gL84Ubzpy_lVyUx9pTVQp7waCgYKAb4SARESFQHGX2MihAta5jxncxfuPyJbGDiDsw0175&token_type=Bearer&expires_in=3599&scope=email%20profile%20openid%20https://www.googleapis.com/auth/userinfo.email%20https://www.googleapis.com/auth/userinfo.profile&authuser=0&prompt=none

                Debug.Log($"fragment1 : {fragment}");
                fragment = fragment.Substring(1);
                Debug.Log($"fragment2 : {fragment}");
                Dictionary<string, string> parameters = new Dictionary<string, string>();

                foreach (string pair in fragment.Split('&'))
                {
                    string[] keyValue = pair.Split('=');
                    if (keyValue.Length == 2)
                    {
                        parameters[keyValue[0]] = Uri.UnescapeDataString(keyValue[1]);
                    }
                }

                if (parameters.ContainsKey("access_token"))
                {
                    string accessToken = parameters["access_token"];
                    FetchUserInfo(accessToken);
                }
                else
                {
                    OnSignInFailed?.Invoke("No access token found in the redirect URL");
                }
            }
            else
            {
                OnSignInFailed?.Invoke("Invalid redirect URL format");
            }
        }
        catch (Exception ex)
        {
            OnSignInFailed?.Invoke($"Failed to extract token: {ex.Message}");
        }
    }

    // 사용자 정보 가져오기
    private void FetchUserInfo(string accessToken)
    {
        MonoSlave.Instance.StartCoroutine(FetchUserInfoCoroutine(accessToken));
    }

    private IEnumerator FetchUserInfoCoroutine(string accessToken)
    {
        using (UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.Get(GOOGLE_USER_INFO_URL))
        {
            request.SetRequestHeader("Authorization", $"Bearer {accessToken}");

            yield return request.SendWebRequest();

            if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                try
                {
                    GoogleUserInfo userInfo = JsonUtility.FromJson<GoogleUserInfo>(json);
                    OnSignInSuccess?.Invoke(userInfo);
                }
                catch (Exception ex)
                {
                    OnSignInFailed?.Invoke($"Failed to parse user info: {ex.Message}");
                }
            }
            else
            {
                OnSignInFailed?.Invoke($"Failed to fetch user info: {request.error}");
            }
        }
    }

    // 서비스 정리
    public void Dispose()
    {
#if !UNITY_EDITOR
        DeepLinkManager.OnDeepLinkReceived -= HandleDeepLink;
#endif

#if UNITY_EDITOR
        EditorOAuthListener.StopListening();
#endif
    }
}

[Serializable]
public class GoogleUserInfo
{
    // 구글 사용자 고유 ID
    public string id;

    // 이메일 주소
    public string email;

    // 이메일 인증 여부
    public bool verified_email;

    // 사용자 이름
    public string name;

    // 사용자 이름(이름 부분)
    public string given_name;

    // 사용자 이름(성 부분)
    public string family_name;

    // 프로필 이미지 URL
    public string picture;

    // 로케일(언어 설정)
    public string locale;
}