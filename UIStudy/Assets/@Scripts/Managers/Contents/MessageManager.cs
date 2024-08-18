using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable] //�갡 �־�� ����ȭ ����
public class MessageData
{
    public string id; // get, set�� ������ �� ��
    public string name;
    public string time; // ���� �� �� ��.
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