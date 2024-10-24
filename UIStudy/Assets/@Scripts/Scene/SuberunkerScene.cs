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

        var inputObject = new GameObject("@Input_Scene");
        var inputScene = inputObject.GetOrAddComponent<Input_SuberunkerScene>();

        Managers.Input.KeyAction -= inputScene.OnKeyAction;
        Managers.Input.KeyAction += inputScene.OnKeyAction;

        _joyStick = Managers.UI.ShowUIBase<UI_Joystick>();
        Managers.Pool.CreatePool(Managers.Resource.Load<GameObject>("Entity"), 10);
        Player = Managers.Object.Spawn<PlayerController>(HardCoding.PlayerStartPos).GetComponent<PlayerController>();
        Managers.Object.Spawn<Teleport>(Vector2.zero);

        Managers.Pool.CreatePool(Managers.Resource.Load<GameObject>("ItemBase"), 10);

        //Spawner라는 객체를 따로만들고

        _difficultySettings = Managers.Data.DifficultySettingsDic[Managers.Game.DifficultySettingsInfo.StageId];

        _spawner = Instantiate(Managers.Resource.Load<GameObject>("Spawner")).GetOrAddComponent<Spawner>();
        _spawner.name = "Spawner";

        StartCoroutine(_spawner.GenerateItem());
        StartCoroutine(_spawner.GenerateStoneCo());

        Managers.Event.AddEvent(EEventType.Attacked_Player, OnEvent_Attacked_Player);

        Managers.Sound.Stop(ESound.Bgm);
        Managers.Sound.Play(Define.ESound.Bgm, "LobbyBGMSound", 0.6f);

        return true;
    }

    private void OnEvent_Attacked_Player(Component sender, object param)
    {
        Managers.Camera.Shake(1.0f, 0.2f);
    }



}