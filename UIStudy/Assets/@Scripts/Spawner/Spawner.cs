using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{

    public IEnumerator GenerateMonster()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.2f);
            int x = UnityEngine.Random.Range(-100, 101);
            Managers.Object.Spawn<MonsterController>(new Vector2(x, 140));
        }
    }

    public IEnumerator GenerateItem()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            int x = UnityEngine.Random.Range(-100, 101);
            Managers.Object.Spawn<SkillItem>(new Vector2(x, -110));
        }
    }
}

