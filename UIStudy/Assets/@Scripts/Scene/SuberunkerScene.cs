using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

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
            player.transform.position = new Vector2(0, -120);
        }
        MonsterInstantiate();

        Managers.Input.KeyAction -= KeyActionEvent;
        Managers.Input.KeyAction += KeyActionEvent;

        return true;
    }

    void KeyActionEvent()
    {
       if(Input.GetKeyDown(KeyCode.A))
        {
            GameObject go = Managers.Pool.Pop(_go);
            int rand = UnityEngine.Random.Range(2, 5);
            go.GetOrAddComponent<MonsterController>().SetInfo(Managers.Data.EntityDic[rand]);
            //MonsterController의 addForce 오류도 안남.
            int x = UnityEngine.Random.Range(-300, 301);
            go.transform.position = new Vector2(x, 140);
        }
        if (Input.GetKeyDown(KeyCode.D)) 
        {
            Managers.Pool.Push(_go);
        }
    }

    private void MonsterInstantiate()
    {
        _go = Managers.Resource.Load<GameObject>("Entity");
        //_go.GetOrAddComponent<MonsterController>().SetInfo(Managers.Data.EntityDic[2]); 
        // 이러면 프리펩에도 MonsterController component가 추가 됨
        _go.name = "enemy";
    }
}