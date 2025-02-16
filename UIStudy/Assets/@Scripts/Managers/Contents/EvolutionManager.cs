using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EvolutionManager
{
    private List<float> _stats = new List<float>();
    public IReadOnlyList<float> ChangedMissionList => _stats;
    private Dictionary<int, float> _dicts = new Dictionary<int, float>();
    public IReadOnlyDictionary<int, float> Dicts => _dicts;

    private List<StatModifier> _modifierList = new List<StatModifier>();
    public List<StatModifier> ModifierList
    {
        get => _modifierList;
        private set
        {
            _modifierList = value;
        }

    }

    public void EvolutionDict()
    {
        int evolutionId = Managers.Game.UserInfo.EvolutionId;
        List<float> stats = Managers.Data.EvolutionDataDic[evolutionId].Stats;
        _dicts = Extension.ListToDict(stats);
    }

    public void SetModifierList()
    {
        List<(EStat, EStatModifierType, float)> options = new List<(EStat, EStatModifierType, float)>
        {
            (EStat.MoveSpeed, EStatModifierType.Flat, Dicts[0]),
            (EStat.MaxHp, EStatModifierType.Flat, Dicts[1]),
            (EStat.Luck, EStatModifierType.Flat, Dicts[2])
        };

        foreach (var option in options)
        {
            if (option.Item3 != 0) // 변화값이 0이 아닌 경우에만
            {
                StatModifier temp = new StatModifier(EStatModifierKind.Passive, option.Item2, option.Item3);
                _modifierList.Add(temp);
            }
        }
    }
}