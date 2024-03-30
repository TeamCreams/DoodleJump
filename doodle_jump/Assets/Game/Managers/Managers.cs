using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Managers : MonoBehaviour
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

    public GameManager GameManager { get; private set; }
    public UIManager UIManager { get; private set; }
    public ObjectManager ObjectManager { get; private set; }

    private void Awake()
    {
        GameManager = (new GameObject($"@{nameof(GameManager)}")).GetOrAddComponent<GameManager>();
        UIManager = (new GameObject($"@{nameof(UIManager)}")).GetOrAddComponent<UIManager>();
        ObjectManager = (new GameObject($"@{nameof(ObjectManager)}")).GetOrAddComponent<ObjectManager>();
    }
}
