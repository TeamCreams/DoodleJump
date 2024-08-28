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

public class SuberunkerScene : BaseScene
{
    private GameObject _go = null;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        Managers.UI.ShowSceneUI<UI_SuberunkerScene>();

        {
            GameObject player = Managers.Resource.Instantiate("Entity");
            player.name = "player";
            player.GetOrAddComponent<PlayerController>().SetInfo(Managers.Data.EntityDic[1]);
            Managers.Game.Life = player.GetOrAddComponent<PlayerController>().Data.Life;
            player.transform.position = new Vector2(0, -120);
        }
        {
            _go = Managers.Resource.Load<GameObject>("Entity");
            //_go.GetOrAddComponent<MonsterController>().SetInfo(Managers.Data.EntityDic[2]); 
            // 이러면 프리펩에도 MonsterController component가 추가 됨
            _go.name = "enemy";
        }
        StartCoroutine(GenerateMonster());
        Managers.Input.KeyAction -= KeyActionEvent;
        Managers.Input.KeyAction += KeyActionEvent;

        return true;
    }

    void KeyActionEvent()
    {
       
    }

    IEnumerator GenerateMonster()
    {
        yield return new WaitForSeconds(0.6f);
        GameObject go = Managers.Pool.Pop(_go);
        int rand = UnityEngine.Random.Range(2, 5);
        go.GetOrAddComponent<MonsterController>().SetInfo(Managers.Data.EntityDic[rand]);
        //MonsterController의 addForce 오류도 안남.
        int x = UnityEngine.Random.Range(-300, 301);
        go.transform.position = new Vector2(x, 140);
        StartCoroutine(GenerateMonster());
    }
}