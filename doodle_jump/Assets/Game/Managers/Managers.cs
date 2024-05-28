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

    public GameManager Game { get; private set; }
    public UIManager UI { get; private set; }
    public ObjectManager Object { get; private set; }
    public ResourceManager Resource { get; private set; } = new ResourceManager();//MonoBehaviour이 아니기 때문
    public PoolManager Pool { get; private set; } = new PoolManager();

    private void Awake()
    {
        Game = (new GameObject($"@{nameof(GameManager)}")).GetOrAddComponent<GameManager>(); ;//MonoBehaviour이기 때문
        UI = (new GameObject($"@{nameof(UIManager)}")).GetOrAddComponent<UIManager>();
        Object = (new GameObject($"@{nameof(ObjectManager)}")).GetOrAddComponent<ObjectManager>();
    }
}
