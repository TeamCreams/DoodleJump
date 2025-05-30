using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class SuberunkerScene : BaseScene
{
    UI_Joystick _joyStick = null;
    Spawner _spawner;
    DifficultySettingsData _difficultySettings;

    public PlayerController Player { get; private set; }

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        Managers.UI.ShowSceneUI<UI_SuberunkerScene>();

        // 게임 난이도 초기화
        Managers.Game.DifficultySettingsInfo.StageId = 70001;
        Managers.Game.DifficultySettingsInfo.StageLevel = 1; // 이 값을 미션 달성에 사용할 때가 있기 떄문에. 그런데 게임을 새로 시작하면 미션 진행도도 초기화가 되기 때문에 이걸 따로 저장해놔야함.
        Managers.Game.DifficultySettingsInfo.AddSpeed = 0;
        Managers.Game.DifficultySettingsInfo.ChallengeScale = 0;
        // 한 게임당 누적 돌 개수 초기화
        Managers.Game.DifficultySettingsInfo.StoneCount = 0;
        

        var inputObject = new GameObject("@Input_Scene");
        var inputScene = inputObject.GetOrAddComponent<Input_SuberunkerScene>();

        Managers.Input.KeyAction -= inputScene.OnKeyAction;
        Managers.Input.KeyAction += inputScene.OnKeyAction;

        _joyStick = Managers.UI.ShowUIBase<UI_Joystick>();
        Managers.Pool.CreatePool(Managers.Resource.Load<GameObject>("Stone"), 80);
        Managers.Pool.CreatePool(Managers.Resource.Load<GameObject>("StoneShard"), 50);

        Player = Managers.Object.Spawn<PlayerController>(HardCoding.PlayerStartPos).GetComponent<PlayerController>();
        Managers.Object.Spawn<Teleport>(Vector2.zero);

        Managers.Pool.CreatePool(Managers.Resource.Load<GameObject>("ItemBase"), 10);

        //Spawner라는 객체를 따로만들고

        _difficultySettings = Managers.Data.DifficultySettingsDic[Managers.Game.DifficultySettingsInfo.StageId];

        _spawner = Instantiate(Managers.Resource.Load<GameObject>("Spawner")).GetOrAddComponent<Spawner>();
        _spawner.name = "Spawner";

        StartCoroutine(_spawner.GenerateItem());
        StartCoroutine(_spawner.GenerateStoneCo());

        Managers.Sound.Stop(ESound.Bgm);
        Managers.Sound.Play(ESound.Bgm, "LobbyBGMSound", 0.6f);

        Managers.Event.AddEvent(EEventType.Attacked_Player, OnEvent_Attacked_Player);
        Managers.Event.RemoveEvent(EEventType.OnPlayerDead, Event_OnPlayerDead);
        Managers.Event.AddEvent(EEventType.OnPlayerDead, Event_OnPlayerDead);

        return true;
    }

    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.Attacked_Player, OnEvent_Attacked_Player);
        Managers.Event.RemoveEvent(EEventType.OnPlayerDead, Event_OnPlayerDead);
    }
#region Events

    private void OnEvent_Attacked_Player(Component sender, object param)
    {
        Managers.Camera.Shake(1.0f, 0.2f);
    }
    void Event_OnPlayerDead(Component sender, object param)
    {
        int tryCount = (int)param;
        var loadingComplete = UI_LoadingPopup.Show();
        if (tryCount == 2)
        {
            loadingComplete.Value = true;
            UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_NetworkSaveError));
            Invoke("ExitGame", 2.5f);
            return;
        }

        Managers.Score.SetScore(
            this, 
            onSuccess: () =>
            {
                Managers.Event.TriggerEvent(EEventType.OnSettlementComplete);
                //Managers.Event.TriggerEvent(EEventType.OnUpdateMission);
                loadingComplete.Value = true;
                Managers.UI.ShowPopupUI<UI_RetryPopup>();
            },
            onFailed: () => Event_OnPlayerDead(this, tryCount++));
    }
    
    #endregion


}