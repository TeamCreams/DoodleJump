using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using UnityEngine;
using static Define;

public class ObjectManager
{
    private HashSet<StoneController> _monsters;
    private List<int> _itemList = new List<int>();

    private GameObject _monsterRoot;
    public Transform MonsterRoot => GetRootTransform("@Monsters");
    public Transform PlayerRoot => GetRootTransform("@Players");
    public Transform ItemRoot => GetRootTransform("@Item");

    public PlayerController Player => _player;
    private PlayerController _player;


    public Transform GetRootTransform(string name)
    {
        GameObject root = GameObject.Find(name);
        if (root == null)
            root = new GameObject { name = name };

        return root.transform;
    }

    public GameObject Spawn<T>(Vector3 pos, bool option = false) where T : ObjectBase
    {
        if (typeof(T) == typeof(StoneController))
        {
            GameObject go = Managers.Resource.Instantiate("Stone", pooling: true);
            int rand = UnityEngine.Random.Range(60001, 60001 + Managers.Data.EnemyDic.Count);
            StoneController stone = go.GetOrAddComponent<StoneController>();
            stone.SetInfo(Managers.Data.EnemyDic[rand]);
            stone.IsNotStoneShower = option;
            stone.Teleport(pos);
            return go;
        }
        else if (typeof(T) == typeof(PlayerController))
        {
            GameObject player = Managers.Resource.Instantiate("Player", pooling: true);
            player.name = "player";
            _player = player.GetOrAddComponent<PlayerController>();
            _player.SetInfo(Managers.Game.ChracterStyleInfo.CharacterId);
            _player.Teleport(pos);
            Managers.Game.Life = _player.Data.Hp;
            return player;
        }
        else if (typeof(T) == typeof(ItemBase))
        {
            GameObject item = Managers.Resource.Instantiate("ItemBase", pooling: true);
            item.name = "ItemBase";
            int randId = RandomItem();
            item.GetOrAddComponent<ItemBase>().SetInfo(randId);
            item.transform.position = pos;

            return item;
        }
        else if (typeof(T) == typeof(Teleport))
        {
            GameObject item = Managers.Resource.Instantiate("TeleportController", pooling: false);

            item.transform.position = pos;

            return item;
        }
        else if (typeof(T) == typeof(StoneShardController))
        {
            GameObject go = Managers.Resource.Instantiate("StoneShard", pooling: true);
            StoneShardController stone = go.GetOrAddComponent<StoneShardController>();
            stone.Teleport(pos);
            return go;
        }
        // else if (typeof(T) == typeof(Confetti_Particle))
        // {
        //     GameObject item = Managers.Resource.Instantiate("Confetti_Particle", pooling: true);
        //     Confetti_Particle particle = item.GetComponent<Confetti_Particle>();
        //     particle.StartParticle(pos);

        //     return item;
        // }

        return null;
    }

    private int RandomItem()
    {
        _itemList.Clear();
        // 전체 확률 합산
        float totalChance = 0f;
        foreach (var item in Managers.Data.SuberunkerItemDic.Values)
        {
            totalChance += item.Chance;
        }
        // 0부터 totalChance 사이에서 랜덤 값 선택
        float roll = UnityEngine.Random.Range(0f, totalChance);
        float cumulative = 0f;

        foreach (var item in Managers.Data.SuberunkerItemDic.Values)
        {
            cumulative += item.Chance;
            if (roll <= cumulative)
            {
                return item.Id;
            }
        }
        // fallback: 마지막 아이템
        return Managers.Data.SuberunkerItemDic.Values.Last().Id;
    }   

    // private int RandomItem()
    // {
    //     _itemList.Clear();
    //     float range = UnityEngine.Random.Range(0, 1.0f);

    //     float min = 1;
    //     float closeValue = 0;

    //     foreach (var item in Managers.Data.SuberunkerItemDic)
    //     {
    //         float difference = Math.Abs(item.Value.Chance - range);
    //         if (difference < min)
    //         {
    //             min = difference;
    //             closeValue = item.Value.Chance;
    //         }
    //         else if(difference == min)
    //         {
    //             if(0 <= item.Value.Chance - range)
    //             {
    //                 min = difference;
    //                 closeValue = item.Value.Chance;
    //             }
    //         }
    //     }

    //     foreach (var item in Managers.Data.SuberunkerItemDic)
    //     {
    //         if(closeValue == item.Value.Chance)
    //         {
    //             _itemList.Add(item.Value.Id);
    //         }
    //     }

    //     int randItem = UnityEngine.Random.Range(0, _itemList.Count);
                
    //     return _itemList[randItem];
    // }
}
