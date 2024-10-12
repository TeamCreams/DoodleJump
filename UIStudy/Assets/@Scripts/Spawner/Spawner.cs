using UnityEngine;
using System.Collections;
using static Define;
using System;

public class Spawner : InitBase
{

    private int _id;
    private float _stoneGenerateTime = 0;
    private float _stoneShowerPeriodTime = 0;

    private Coroutine _generateStone;
    private Coroutine _generateStoneShower;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        _id = Managers.Game.DifficultySettingsInfo.StageId;
        _stoneGenerateTime = UnityEngine.Random.Range(Managers.Data.DifficultySettingsDic[_id].StoneGenerateStartTime, Managers.Data.DifficultySettingsDic[_id].StoneGenerateFinishTime);
        _stoneShowerPeriodTime = UnityEngine.Random.Range(Managers.Data.DifficultySettingsDic[_id].StoneShowerPeriodStartTime, Managers.Data.DifficultySettingsDic[_id].StoneShowerPeriodFinishTime);

        Managers.Event.AddEvent(EEventType.LevelStageUp, OnEvent_LevelStageUp);

        return true;
    }

    /*
        1. 일반 돌이 생성되며, 이것은 피한 돌로 카운트 됨
        2. 돌샤워가 진행될 시간이 되면 일반 돌이 생성되는 함수가 멈춤.
     */


    public IEnumerator GenerateItem()
    {
        while (true)
        {
            int time = UnityEngine.Random.Range(7, 12); // 수치화 필요   
            yield return new WaitForSeconds(time);
            int x = UnityEngine.Random.Range(-90, 91);
            Managers.Object.Spawn<ItemBase>(new Vector2(x, -110));
        }
    }
    public IEnumerator GenerateTestCor()
    {
        while(true)
        {
            if (_generateStone == null && _generateStoneShower == null)
            {

                Debug.Log("hihihihihihihihihi");
                _generateStone = StartCoroutine(GenerateStone());
                yield return new WaitForSeconds(_stoneShowerPeriodTime);
            }


            if (_generateStoneShower == null)
            {
                if (_generateStone != null)
                {
                    StopCoroutine(_generateStone);
                    _generateStone = null;
                }
                _generateStoneShower = StartCoroutine(GenerateStoneShower());
            }
            yield return null;
        }
    }

    public IEnumerator GenerateStone()
    {
        if (_generateStoneShower != null)
        {
            StopCoroutine(_generateStoneShower);
        }
        while (true)
        {
            yield return new WaitForSeconds(_stoneGenerateTime);

            int x = UnityEngine.Random.Range(-90, 91);
            Managers.Object.Spawn<StoneController>(new Vector2(x, 140));

            Managers.Game.DifficultySettingsInfo.ChallengeScale++;
            if (Managers.Data.DifficultySettingsDic[_id].ChallengeScale <= Managers.Game.DifficultySettingsInfo.ChallengeScale)
            {
                Managers.Event.TriggerEvent(EEventType.LevelStageUp);
            }
        }
    }

    public IEnumerator GenerateStoneShower()
    {
        int direction = UnityEngine.Random.Range(0, 2) * 2 - 1;
        int reversDirection = direction * -1;
        int startX = 90 * direction;
        int endX = 90 * reversDirection; 
        int reversDirectionDistance = reversDirection * 7;
        while ((direction == 1 && endX <= startX) || (direction == -1 && startX <= endX))
        {
            Managers.Object.Spawn<StoneController>(new Vector2(startX, 140));
            startX += reversDirectionDistance;
            yield return new WaitForSeconds(0.2f);
        }
        _stoneShowerPeriodTime = UnityEngine.Random.Range(Managers.Data.DifficultySettingsDic[_id].StoneShowerPeriodStartTime, Managers.Data.DifficultySettingsDic[_id].StoneShowerPeriodFinishTime);
        _generateStoneShower = null;
    }


    private void OnEvent_LevelStageUp(Component sender, object param)
    {
        Managers.Game.DifficultySettingsInfo.StageId++;
        _id = Managers.Game.DifficultySettingsInfo.StageId;
        _stoneGenerateTime = UnityEngine.Random.Range(Managers.Data.DifficultySettingsDic[_id].StoneGenerateStartTime, Managers.Data.DifficultySettingsDic[_id].StoneGenerateFinishTime);
        _stoneShowerPeriodTime = UnityEngine.Random.Range(Managers.Data.DifficultySettingsDic[_id].StoneShowerPeriodStartTime, Managers.Data.DifficultySettingsDic[_id].StoneShowerPeriodFinishTime);
    }
}

