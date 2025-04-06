using Data;
using Firebase.Auth;
using Google;
using Gpm.WebView;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public void SignIn()
    {
        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            RequestIdToken = true,
            // Copy this value from the google-service.json file.
            // oauth_client with type == 3
            WebClientId = "550559090082-5fchrj8tj3arltl2ktv615hla7f3veat.apps.googleusercontent.com"
        };

        Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();

        TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();
        signIn.ContinueWith(task => {
            if (task.IsCanceled)
            {
                signInCompleted.SetCanceled();
            }
            else if (task.IsFaulted)
            {
                signInCompleted.SetException(task.Exception);
            }
            else
            {

                Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(((Task<GoogleSignInUser>)task).Result.IdToken, null);
                FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(credential).ContinueWith(async authTask => {
                    if (authTask.IsCanceled)
                    {
                        signInCompleted.SetCanceled();
                    }
                    else if (authTask.IsFaulted)
                    {
                        signInCompleted.SetException(authTask.Exception);
                    }
                    else
                    {
                        signInCompleted.SetResult(((Task<FirebaseUser>)authTask).Result);

                        var token = await ((Task<FirebaseUser>)authTask).Result.TokenAsync(false);

                    }
                });
            }
        });
    }
}
