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

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        Managers.UI.ShowSceneUI<UI_SuberunkerScene>();
        _joyStick = Managers.UI.ShowUIBase<UI_Joystick>();
        Managers.Pool.CreatePool(Managers.Resource.Load<GameObject>("Entity"), 10);
        Managers.Object.Spawn<PlayerController>(HardCoding.PlayerStartPos);
        Managers.Object.Spawn<TeleportController>(Vector2.zero);

        Managers.Pool.CreatePool(Managers.Resource.Load<GameObject>("ItemBase"), 10);

        //Spawner라는 객체를 따로만들고

        _difficultySettings = Managers.Data.DifficultySettingsDic[Managers.Game.DifficultySettingsInfo.StageId];

        _spawner = Instantiate(Managers.Resource.Load<GameObject>("Spawner")).GetOrAddComponent<Spawner>();

        StartCoroutine(_spawner.GenerateItem());
        StartCoroutine(_spawner.GenerateStone());
        return true;
    }

    IEnumerator StartStoneShower()
    {
        /* 주기적으로 확인이 필요한 정보 
        _difficultySettings = Managers.Data.DifficultySettingsDic[Managers.Game.DifficultySettingsInfo.StageId];

        if (_difficultySettings.StoneShower != 0)
        {

        }
        */
        yield return new WaitForSeconds(_difficultySettings.StoneShower);
        StopCoroutine(_spawner.GenerateStone());
        Managers.Game.DifficultySettingsInfo.StoneShower++;
        StartCoroutine(_spawner.GenerateStoneShower());
    }


}