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
	private LanguageDataManager _language = new LanguageDataManager();
    private ScoreManager _score = new ScoreManager();
    private WebContentsManager _webContents = new WebContentsManager();
	private MissionManager _mission = new MissionManager();
	private ErrorManager _error = new ErrorManager();
	private EvolutionManager _evolution = new EvolutionManager();
	private ChattingManager _chatting = new ChattingManager();
	private IAPManager _iap = new IAPManager();
	private LoginManager _login = new LoginManager();

    public static GameManager Game { get { return Instance?._game; } }
	public static MessageManager Message { get { return Instance?._message; } }
	public static DataManager Data { get { return Instance?._data; } }
	public static ObjectManager Object {  get { return Instance?._object; } }
	public static LanguageDataManager Language { get { return Instance?._language; } }
	public static ScoreManager Score { get { return Instance?._score; } }
    public static WebContentsManager WebContents { get { return Instance?._webContents; } }
    public static MissionManager Mission { get { return Instance?._mission; } }
	public static ErrorManager Error { get { return Instance?._error; } }
	public static EvolutionManager Evolution { get { return Instance?._evolution; } }
	public static ChattingManager Chatting {get {return Instance?._chatting;}}
	public static IAPManager IAP { get { return Instance?._iap; } }
	public static LoginManager Login {get {return Instance?._login; }}

    #endregion

    #region Core
    private PoolManager _pool = new PoolManager();
	private ResourceManager _resource = new ResourceManager();
	private SceneManagerEx _scene = new SceneManagerEx();
	private SoundManager _sound = new SoundManager();
	private InputManagerEx _input = new InputManagerEx();
	private UIManager _ui = new UIManager(); 
	private EventManager _event = new EventManager();
	private CameraManager _camera = new CameraManager();
	private WebManager _web = new WebManager();
	private SignalRManager _signalR = new SignalRManager();

	public static PoolManager Pool { get { return Instance?._pool; } }
	public static ResourceManager Resource { get { return Instance?._resource; } }
	public static SceneManagerEx Scene { get { return Instance?._scene; } }
	public static SoundManager Sound { get { return Instance?._sound; } }
	public static InputManagerEx Input { get { return Instance?._input; } }
	public static UIManager UI { get { return Instance?._ui; } } 
	public static EventManager Event { get {  return Instance?._event; } }
	public static CameraManager Camera { get { return Instance?._camera; } }
	public static WebManager Web { get { return Instance?._web; } }
	public static SignalRManager SignalR { get { return Instance?._signalR; } }

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

			Systems.Init();
			
			// 초기화
			s_instance = go.GetComponent<Managers>();

			Managers.Event.Init();
			Managers.Game.Init();
			Managers.Sound.Init();
			Managers.Web.Init();
			Managers.Mission.Init();
			Managers.Login.Init();
			Managers.SignalR.InitAsync();



			//LATE INIT
			Managers.IAP.LateInit();


            Managers.Event.RemoveEvent(Define.EEventType.OnLogout, OnEvent_Logout);
			Managers.Event.AddEvent(Define.EEventType.OnLogout, OnEvent_Logout);
        }
	}

	public static void OnEvent_Logout(Component sender, object param)
    {
        Managers.Event.Init();
        Managers.Game.Init();
        Managers.Sound.Init();
        Managers.Web.Init();
        Managers.Mission.Init();
		Managers.Login.Init();
    }
	
	// public static void InitScene()
    // {
    //     Managers.Score.Init();
    // }

    private void Update()
    {
		Input.OnUpdate();
    }

	public static void Clear()
	{
        Pool.Clear();
		//Event.Clear();
	}
}

// 1. 애니메이션
// 2. 게임 이쁘게
//    - 포트폴리오 용으로 만들거라서
// 3. 스탯, 스킬하는거 붙여서  (액티브, 패시브)
// 4. 캐릭터 종류
// 리소스 
