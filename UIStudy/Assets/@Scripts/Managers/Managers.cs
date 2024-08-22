using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Managers : MonoBehaviour
{
	private static Managers s_instance;
	private static Managers Instance { get { Init(); return s_instance; } }
    #region Contents
    private GameManager _game = new GameManager();

    private MessageManager _message = new MessageManager();
	private DataManager _data = new DataManager();

    public static GameManager Game { get { return Instance?._game; } }
	public static MessageManager Message { get { return Instance?._message; } }
	public static DataManager Data { get { return Instance?._data; } }
    #endregion

    #region Core
    private PoolManager _pool = new PoolManager();
	private ResourceManager _resource = new ResourceManager();
	private SceneManagerEx _scene = new SceneManagerEx();
	private SoundManager _sound = new SoundManager();
	private InputManagerEx _input = new InputManagerEx();
	private UIManager _ui = new UIManager(); 

	public static PoolManager Pool { get { return Instance?._pool; } }
	public static ResourceManager Resource { get { return Instance?._resource; } }
	public static SceneManagerEx Scene { get { return Instance?._scene; } }
	public static SoundManager Sound { get { return Instance?._sound; } }
	public static InputManagerEx Input { get { return Instance?._input; } }
	public static UIManager UI { get { return Instance?._ui; } } 
    #endregion


    public static void Init()
	{
		if (s_instance == null)
		{
			GameObject go = GameObject.Find("@Managers");
			if (go == null)
			{
				go = new GameObject { name = "@Managers" };
				go.AddComponent<Managers>();
			}

			DontDestroyOnLoad(go);

			// 초기화
			s_instance = go.GetComponent<Managers>();
		}
	}

    private void Update()
    {
		Input.OnUpdate();
    }

}
