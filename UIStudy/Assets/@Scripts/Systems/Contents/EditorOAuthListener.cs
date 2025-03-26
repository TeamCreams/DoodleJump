
// 에디터용 OAuth 리스너 구현
#if UNITY_EDITOR
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public static class EditorOAuthListener
{
    private static HttpListener httpListener;
    private static Thread listenerThread;
    private static Action<string> callbackAction;
    private static bool isListening = false;

    public static void StartListening(string redirectUri, Action<string> callback)
    {
        if (isListening)
        {
            Debug.LogWarning("OAuth listener is already running");
            return;
        }

        try
        {
            Uri uri = new Uri(redirectUri);
            string prefix = $"http://{uri.Host}:{uri.Port}/";

            httpListener = new HttpListener();
            httpListener.Prefixes.Add(prefix);
            httpListener.Start();

            callbackAction = callback;
            isListening = true;

            listenerThread = new Thread(ListenerThread);
            listenerThread.Start();

            Debug.Log($"Started OAuth listener on {prefix}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to start OAuth listener: {ex.Message}");
        }
    }

    public static void StopListening()
    {
        if (!isListening) return;

        isListening = false;

        try
        {
            httpListener?.Stop();
            listenerThread?.Join(500);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error stopping OAuth listener: {ex.Message}");
        }

        httpListener = null;
        listenerThread = null;
        callbackAction = null;
    }
    private static void ListenerThread()
    {
        while (isListening)
        {
            try
            {
                HttpListenerContext context = httpListener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                // URL에서 token 쿼리 파라미터 추출
                string requestUrl = request.Url.ToString();

                Task task = new Task(async () =>
                {
                    Debug.Log($"requestUrl : {requestUrl}");
                    await Awaitable.MainThreadAsync();
                    callbackAction?.Invoke(requestUrl);
                });
                task.Start();

                Thread.Sleep(1000);
                StopListening();

                //if (requestUrl.Contains("?token="))
                //{
                //    string[] parts = requestUrl.Split(new string[] { "?token=" }, StringSplitOptions.None);
                //    if (parts.Length > 1)
                //    {
                //        string encodedToken = parts[1];
                //        // URL 디코딩
                //        string decodedFragment = Uri.UnescapeDataString(encodedToken);

                //        // 이제 decodedFragment는 "access_token=xxx&token_type=Bearer&..." 형태
                //        // 여기서 access_token 값을 추출
                //        string[] parameters = decodedFragment.Split('&');
                //        foreach (string parameter in parameters)
                //        {
                //            string[] keyValue = parameter.Split('=');
                //            if (keyValue.Length == 2 && keyValue[0] == "access_token")
                //            {
                //                string accessToken = keyValue[1];
                //                Debug.Log(accessToken);

                //                // 이제 accessToken 사용
                //            }
                //        }
                //    }
                //}

                string scriptResponseString = @"
            <html>
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
              </head>
            <body>

            </body>
            </html>";
                /*
                 *                 <script>
                    // URL에서 프래그먼트를 추출
                    var hash = window.location.hash.substring(1);
                    if (hash) {
                        // 다시 서버로 쿼리 파라미터로 전송
                        window.location.href = window.location.origin + window.location.pathname + '?token=' + encodeURIComponent(hash);
                    } else {
                        document.write('<h1>인증 진행 중...</h1>');
                    }
                </script>
                 */
                byte[] scriptBuffer = System.Text.Encoding.UTF8.GetBytes(scriptResponseString);

                response.ContentType = "text/html";
                response.ContentLength64 = scriptBuffer.Length;
                response.OutputStream.Write(scriptBuffer, 0, scriptBuffer.Length);
                response.OutputStream.Close();
            }
            catch (Exception ex)
            {
                if (isListening)
                {
                    Debug.LogError($"Error in OAuth listener thread: {ex.Message}");
                }
                break;
            }
        }
    }
}
#endif