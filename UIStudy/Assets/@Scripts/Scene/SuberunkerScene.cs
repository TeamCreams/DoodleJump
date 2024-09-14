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
        StartCoroutine(GenerateMonster());
        return true;
    }



    IEnumerator GenerateMonster()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.6f);
			int x = UnityEngine.Random.Range(-300, 301);
            Managers.Object.Spawn<MonsterController>(new Vector2(x, 140));
        }
    }
}