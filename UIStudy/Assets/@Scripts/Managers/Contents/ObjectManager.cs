using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using UnityEngine;

public class ObjectManager
{
    private HashSet<MonsterController> _monsters;

    private GameObject _monsterRoot;
    public Transform MonsterRoot => GetRootTransform("@Monsters");
    public Transform PlayerRoot => GetRootTransform("@Players");
    public Transform ItemRoot => GetRootTransform("@Item");

    public Transform GetRootTransform(string name)
    {
        GameObject root = GameObject.Find(name);
        if (root == null)
            root = new GameObject { name = name };

        return root.transform;
    }

    public void Spawn<T>(Vector2 pos) where T : ObjectBase
    {
        if (typeof(T) == typeof(MonsterController))
        {
            GameObject go = Managers.Resource.Instantiate("Entity", pooling: true);
            int rand = UnityEngine.Random.Range(1, 5);
            go.GetOrAddComponent<MonsterController>().SetInfo(Managers.Data.EnemyDic[rand]);
            go.transform.position = pos;

            //go.transform.parent = MonsterRoot;
        }
        else if (typeof(T) == typeof(PlayerController))
        {
            GameObject player = Managers.Resource.Instantiate("Player", pooling: true);
            player.name = "player";
            //Debug.Log(Managers.Game.ChracterStyleInfo.CharacterId);
            player.GetOrAddComponent<PlayerController>().SetInfo(Managers.Game.ChracterStyleInfo.CharacterId);
            player.GetOrAddComponent<PlayerController>().Teleport();
            Managers.Game.Life = player.GetOrAddComponent<PlayerController>().Data.Life;

            //player.transform.parent = PlayerRoot;
        }
        else if (typeof(T) == typeof(ItemBase))
        {
            GameObject item = Managers.Resource.Instantiate("ItemBase", pooling: true);
            item.name = "ItemBase";
            int randId = UnityEngine.Random.Range(30001, 30001 + Managers.Data.SuberunkerItemDic.Count);
            item.GetOrAddComponent<ItemBase>().SetInfo(randId);
            item.transform.position = pos;
        }

    }

}
