using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public InGameScene CurrentScene { get; set; } = null;


    private void Awake()
    {
        CurrentScene = GameObject.FindObjectOfType<InGameScene>();
    }


    public void Run()
    {
        Debug.Log("GameManager Run");
    }
}