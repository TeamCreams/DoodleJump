using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable] //얘가 있어야 직렬화 가능
public class MessageData
{
    public string id; // get, set도 있으면 안 됨
    public string name;
    public string time; // 아직 안 쓸 듯.
    public string message;
}

[System.Serializable]
public class Messages 
{
    public List<MessageData> Chatting;
}


public class MessageManager
{
    void ReadTextFile(string path = null)
    {
        if (path == null)
        {
            path = "Message";
        }
        TextAsset file = Resources.Load<TextAsset>($"Kakao/Text/{path}");
        if (file == null)
        {
            Debug.LogError("File not found!");
            return;
        }

        Messages messages = JsonUtility.FromJson<Messages>(file.text);

        if (messages.Chatting == null)
        {
            Debug.Log("is NULL");
            return;
        }
        else
        {
            foreach (var message in messages.Chatting)
            {
                Debug.Log(message.id);
                Debug.Log(message.name);
                Debug.Log(message.time);
                Debug.Log(message.message);
            }
        }
    }

}