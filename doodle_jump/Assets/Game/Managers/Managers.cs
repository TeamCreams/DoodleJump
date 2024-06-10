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
                _instance = FindObjectOfType<Managers>();
                if (_instance == null)
                {
                    var temp = new GameObject("@Managers");
                    _instance = temp.GetOrAddComponent<Managers>();
                }
            }
            return _instance;
        }
    }
    private GameManager _game;
    private UIManager _ui;
    private ResourceManager _resource = new ResourceManager();
    private PoolManager _pool = new PoolManager();


    public static GameManager Game => Instance._game;
    public static UIManager UI => Instance._ui;
    public static ResourceManager Resource => Instance._resource;
    public static PoolManager Pool => Instance._pool;

    public static Game2048 Game2048 => Instance._game2048;

    [SerializeField]
    private Game2048 _game2048;


    private void Awake()
    {
        _game = (new GameObject($"@{nameof(GameManager)}")).GetOrAddComponent<GameManager>();
        _ui = (new GameObject($"@{nameof(UIManager)}")).GetOrAddComponent<UIManager>();

        _game2048 = this.GetComponent<Game2048>();
    }
}
