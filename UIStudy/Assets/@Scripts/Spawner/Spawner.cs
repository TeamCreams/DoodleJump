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
        Managers.Game.DifficultySettingsInfo.ChallengeScaleCount = Managers.Data.DifficultySettingsDic[_id].ChallengeScale;
        _stoneGenerateTime = UnityEngine.Random.Range(Managers.Data.DifficultySettingsDic[_id].StoneGenerateStartTime, Managers.Data.DifficultySettingsDic[_id].StoneGenerateFinishTime);
        _stoneShowerPeriodTime = UnityEngine.Random.Range(Managers.Data.DifficultySettingsDic[_id].StoneShowerPeriodStartTime, Managers.Data.DifficultySettingsDic[_id].StoneShowerPeriodFinishTime);

        Managers.Event.AddEvent(EEventType.LevelStageUp, OnEvent_LevelStageUp);
        Managers.Event.TriggerEvent(EEventType.UIStoneCountRefresh);

        return true;
    }
    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.LevelStageUp, OnEvent_LevelStageUp);
    }
    public IEnumerator GenerateItem()
    {
        while (true)
        {
            int time = UnityEngine.Random.Range(10, 15); // 수치화 필요   
            yield return new WaitForSeconds(time);
            int x = UnityEngine.Random.Range(-85, 86);
            Managers.Object.Spawn<ItemBase>(new Vector2(x, -110));
        }
    }
    public IEnumerator GenerateStoneCo()
    {
        Managers.Event.TriggerEvent(EEventType.IsStoneShower);
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
                Managers.Game.DifficultySettingsInfo.StageLevel = Managers.Data.DifficultySettingsDic[Managers.Game.DifficultySettingsInfo.StageId].Level; // 함수 호출 순서 때문에 여기서 부름
                Debug.Log($"GenerateStone StageLevel :  {Managers.Game.DifficultySettingsInfo.StageLevel}");
                Managers.Event.TriggerEvent(EEventType.LevelStageUp);
            }
            _stoneGenerateTime = UnityEngine.Random.Range(Managers.Data.DifficultySettingsDic[_id].StoneGenerateStartTime, Managers.Data.DifficultySettingsDic[_id].StoneGenerateFinishTime);
        }
    }

    public IEnumerator GenerateStoneShower()
    {
        Managers.Event.TriggerEvent(EEventType.IsStoneShower);
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
        this.LevelStageUp();
    }

    private void LevelStageUp()
    {
        //0. 베네핏 점수
        Managers.Game.GetScore.Total += Managers.Data.DifficultySettingsDic[_id].Benefit;

        //1. 레벨업 조건 초기화
        Managers.Game.DifficultySettingsInfo.ChallengeScale = 0;
        _id = Managers.Game.DifficultySettingsInfo.StageId;
        Managers.Game.DifficultySettingsInfo.ChallengeScaleCount = Managers.Data.DifficultySettingsDic[_id].ChallengeScale;
        Managers.Event.TriggerEvent(EEventType.UIStoneCountRefresh);

        //2. 레벨에 따른 난이도 세팅 
        _stoneGenerateTime = UnityEngine.Random.Range(Managers.Data.DifficultySettingsDic[_id].StoneGenerateStartTime, Managers.Data.DifficultySettingsDic[_id].StoneGenerateFinishTime);
        _stoneShowerPeriodTime = UnityEngine.Random.Range(Managers.Data.DifficultySettingsDic[_id].StoneShowerPeriodStartTime, Managers.Data.DifficultySettingsDic[_id].StoneShowerPeriodFinishTime);
        Managers.Game.DifficultySettingsInfo.AddSpeed = 4 * Managers.Game.DifficultySettingsInfo.StageLevel;

        //3. 레벨업 팝업
        //Managers.Object.Spawn<Confetti_Particle>(HardCoding.ConfetiParticlePos);
        Managers.UI.ShowPopupUI<UI_LevelUpPopup>();

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

