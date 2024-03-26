using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers
{
    private static Managers _instance = null;
    public static Managers Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Managers();
            }
            return _instance;
        }
    }

    public GameManager GameManager { get; private set; } = new GameManager();
    public UIManager UIManager { get; private set; } = new UIManager();
    public ObjectManager ObjectManager { get; private set; } = new ObjectManager();

    public void Run()
    {
        Debug.Log("Run");
    }
}
