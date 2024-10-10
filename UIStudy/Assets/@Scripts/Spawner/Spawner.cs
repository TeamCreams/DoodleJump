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

    public IEnumerator GenerateStone()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.2f);
            int x = UnityEngine.Random.Range(-90, 91);
            Managers.Object.Spawn<StoneController>(new Vector2(x, 140));

            Managers.Game.DifficultySettingsInfo.ChallengeScale++;
            int id = Managers.Game.DifficultySettingsInfo.StageId;
            if (Managers.Data.DifficultySettingsDic[id].ChallengeScale <= Managers.Game.DifficultySettingsInfo.ChallengeScale)
            {
                Managers.Game.DifficultySettingsInfo.StageId++;
            }
        }
    }

    public IEnumerator GenerateItem()
    {
        while (true)
        {
            int time = UnityEngine.Random.Range(7, 12);
            yield return new WaitForSeconds(time);
            int x = UnityEngine.Random.Range(-90, 91);
            Managers.Object.Spawn<ItemBase>(new Vector2(x, -110));
        }
    }

    public IEnumerator GenerateStoneShower()
    {
        int x = -90; // 좌우 변경 가능해야할 듯  
        while (x < 91)
        {
            x += 2;
            yield return new WaitForSeconds(0.5f);
            Managers.Object.Spawn<StoneController>(new Vector2(x, 140));
        }
        StartCoroutine(this.GenerateStone());

    }


}

