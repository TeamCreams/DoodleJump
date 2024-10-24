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

    public IEnumerator GenerateItem()
    {
        while (true)
        {
            int time = UnityEngine.Random.Range(7, 12); // 수치화 필요   
            yield return new WaitForSeconds(time);
            int x = UnityEngine.Random.Range(-85, 86);
            Managers.Object.Spawn<ItemBase>(new Vector2(x, -110));
        }
    }
    public IEnumerator GenerateStoneCo()
    {
        while(true)
        {
            if (_generateStone == null && _generateStoneShower == null)
            {
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

            int x = UnityEngine.Random.Range(-85, 86);
            Managers.Object.Spawn<StoneController>(new Vector2(x, 140), true);

            if (Managers.Data.DifficultySettingsDic[_id].ChallengeScale <= Managers.Game.DifficultySettingsInfo.ChallengeScale)
            {
                Managers.Game.DifficultySettingsInfo.StageId++;
                Managers.Game.DifficultySettingsInfo.StageLevel++; // 함수 호출 순서 때문에 여기서 부름
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
            var stoneObject = Managers.Object.Spawn<StoneController>(new Vector2(startX, 140));
            startX += reversDirectionDistance;
            yield return new WaitForSeconds(0.2f);
        }
        _stoneShowerPeriodTime = UnityEngine.Random.Range(Managers.Data.DifficultySettingsDic[_id].StoneShowerPeriodStartTime, Managers.Data.DifficultySettingsDic[_id].StoneShowerPeriodFinishTime);
        _generateStoneShower = null;
    }


    private void OnEvent_LevelStageUp(Component sender, object param)
    {
        //1. 레벨업 조건 초기화
        Managers.Game.DifficultySettingsInfo.ChallengeScale = 0;
        _id = Managers.Game.DifficultySettingsInfo.StageId;

        //2. 레벨에 따른 난인도 세팅 
        _stoneGenerateTime = UnityEngine.Random.Range(Managers.Data.DifficultySettingsDic[_id].StoneGenerateStartTime, Managers.Data.DifficultySettingsDic[_id].StoneGenerateFinishTime);
        _stoneShowerPeriodTime = UnityEngine.Random.Range(Managers.Data.DifficultySettingsDic[_id].StoneShowerPeriodStartTime, Managers.Data.DifficultySettingsDic[_id].StoneShowerPeriodFinishTime);
        Managers.Game.DifficultySettingsInfo.AddSpeed = 4 * Managers.Game.DifficultySettingsInfo.StageLevel;

        //3. 레벨업 팝업
        Managers.Object.Spawn<Confetti_Particle>(HardCoding.ConfetiParticlePos);

        //4. 돌 이벤트 관련 세팅
        if (_generateStoneShower != null)
        {
            StopCoroutine(_generateStoneShower);
            _generateStoneShower = null;
        }
        StopCoroutine(GenerateStoneCo());
        if (_generateStone != null)
        {
            StopCoroutine(_generateStone);
            _generateStone = null;
        }
        StartCoroutine(GenerateStoneCo());
    }
}

