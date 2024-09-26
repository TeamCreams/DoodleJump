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
	private ObjectManager _object = new ObjectManager();

	public static GameManager Game { get { return Instance?._game; } }
	public static MessageManager Message { get { return Instance?._message; } }
	public static DataManager Data { get { return Instance?._data; } }
	public static ObjectManager Object {  get { return Instance?._object; } }
    #endregion

    #region Core
    private PoolManager _pool = new PoolManager();
	private ResourceManager _resource = new ResourceManager();
	private SceneManagerEx _scene = new SceneManagerEx();
	private SoundManager _sound = new SoundManager();
	private InputManagerEx _input = new InputManagerEx();
	private UIManager _ui = new UIManager(); 
	private EventManager _event = new EventManager();

	public static PoolManager Pool { get { return Instance?._pool; } }
	public static ResourceManager Resource { get { return Instance?._resource; } }
	public static SceneManagerEx Scene { get { return Instance?._scene; } }
	public static SoundManager Sound { get { return Instance?._sound; } }
	public static InputManagerEx Input { get { return Instance?._input; } }
	public static UIManager UI { get { return Instance?._ui; } } 
	public static EventManager Event { get {  return Instance?._event; } }
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

			Managers.Event.Init();
			Managers.Game.Init();
		}
	}


    private void Update()
    {
		Input.OnUpdate();
    }

	public static void Clear()
	{
        Pool.Clear();
		Event.Clear();
	}
}

// 1. 애니메이션
// 2. 게임 이쁘게
//    - 포트폴리오 용으로 만들거라서
// 3. 스탯, 스킬하는거 붙여서  (액티브, 패시브)
// 4. 캐릭터 종류
// 리소스 
