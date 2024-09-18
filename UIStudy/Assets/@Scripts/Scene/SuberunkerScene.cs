using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class SuberunkerScene : BaseScene
{
    UI_Joystick _joyStick = null;
    Spawner _spawner;
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

        Managers.Pool.CreatePool(Managers.Resource.Load<GameObject>("SkillItem"), 10);

        //Spawner라는 객체를 따로만들고
        _spawner = Instantiate(Managers.Resource.Load<GameObject>("Spawner")).GetOrAddComponent<Spawner>();

        StartCoroutine(_spawner.GenerateItem());
        StartCoroutine(_spawner.GenerateMonster());
        return true;
    }

}