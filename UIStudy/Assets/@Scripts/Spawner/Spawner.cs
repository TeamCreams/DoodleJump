using UnityEngine;
using System.Collections;

public class Spawner : InitBase
{
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        return true;
    }
    public IEnumerator GenerateMonster()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.2f);
            int x = UnityEngine.Random.Range(-100, 101);
            Managers.Object.Spawn<StoneController>(new Vector2(x, 140));
        }
    }

    public IEnumerator GenerateItem()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            int x = UnityEngine.Random.Range(-100, 101);
            Managers.Object.Spawn<ItemBase>(new Vector2(x, -110));
        }
    }
}

