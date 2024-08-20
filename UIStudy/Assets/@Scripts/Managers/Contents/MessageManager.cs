using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MessageManager
{
    public Messages ReadTextFile(string path = null)
    {
        if (path == null)
        {
            path = "Message";
        }
        TextAsset file = Resources.Load<TextAsset>($"Kakao/Text/{path}");
        if (file == null)
        {
            Debug.LogError("File not found!");
            return null;
        }

        Messages messages = JsonUtility.FromJson<Messages>(file.text);

        if (messages.Chatting == null)
        {
            Debug.Log("is NULL");
            return null;
        }
        else
        {
            return messages;
            /*
             foreach (var message in messages.Chatting)
            {
                Debug.Log(message.id);
                Debug.Log(message.name);
                Debug.Log(message.time);
                Debug.Log(message.message);
            }
             */
        }
    }

}