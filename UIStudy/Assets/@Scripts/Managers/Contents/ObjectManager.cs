using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ObjectManager
{
	private HashSet<MonsterController> _monsters;

	private GameObject _monsterRoot;
    public Transform MonsterRoot => GetRootTransform("@Monsters");
    public Transform PlayerRoot => GetRootTransform("@Players");

    public Transform GetRootTransform(string name)
    {
        GameObject root = GameObject.Find(name);
        if (root == null)
            root = new GameObject { name = name };

        return root.transform;
    }

    public void Spawn<T>(Vector2 pos) where T : ObjectBase
	{
        if(typeof(T) ==  typeof(MonsterController))
		{
            GameObject go = Managers.Resource.Instantiate("Entity");
            int rand = UnityEngine.Random.Range(2, 5);
            go.GetOrAddComponent<MonsterController>().SetInfo(Managers.Data.EntityDic[rand]);
            go.transform.position = pos;

            go.transform.parent = MonsterRoot;
        }
        else if(typeof(T) == typeof(PlayerController))
		{
            GameObject player = Managers.Resource.Instantiate("Entity", pooling: true);
            player.name = "player";
            player.GetOrAddComponent<PlayerController>().SetInfo(Define.Constants.PLAYER_ID);
            Managers.Game.Life = player.GetOrAddComponent<PlayerController>().Data.Life;
            player.transform.position = pos;

            player.transform.parent = PlayerRoot;
        }
	}

}
