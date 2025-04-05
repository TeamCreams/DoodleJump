using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using Data;

public interface ILoader<Key, Value>
{
	Dictionary<Key, Value> MakeDict();
}
public class DataManager
{
    #region About IAP
    public Dictionary<string, Data.IapProductData> IapDic { get; private set; } = new Dictionary<string, Data.IapProductData>();
    #endregion


    public Dictionary<int, Data.TestData> TestDic { get; private set; } = new Dictionary<int, Data.TestData>();
    public Dictionary<int, Data.TinyFarmData> TinyFarmDic { get; private set; } = new Dictionary<int, Data.TinyFarmData>();
    public Dictionary<int, Data.EnemyData> EnemyDic { get; private set; } = new Dictionary<int, Data.EnemyData>();
    public Dictionary<int, Data.PlayerData> PlayerDic { get; private set; } = new Dictionary<int, Data.PlayerData>();
    public Dictionary<int, Data.CharacterItemSpriteData> CharacterItemSpriteDic { get; private set; } = new Dictionary<int, Data.CharacterItemSpriteData>();
    public Dictionary<int, Data.SuberunkerItemData> SuberunkerItemDic { get; private set; } = new Dictionary<int, Data.SuberunkerItemData>();
    public Dictionary<int, Data.SuberunkerItemSpriteData> SuberunkerItemSpriteDic { get; private set; } = new Dictionary<int, Data.SuberunkerItemSpriteData>();
    public Dictionary<int, Data.DifficultySettingsData> DifficultySettingsDic { get; private set; } = new Dictionary<int, Data.DifficultySettingsData>();
    public Dictionary<int, Data.ThoughtBubbleData> ThoughtBubbleDataDic { get; private set; } = new Dictionary<int, Data.ThoughtBubbleData>();
    public Dictionary<int, Data.ThoughtBubbleLanguageData> ThoughtBubbleLanguageDataDic { get; private set; } = new Dictionary<int, Data.ThoughtBubbleLanguageData>();
    public Dictionary<int, Data.GameLanguageData> GameLanguageDataDic { get; private set; } = new Dictionary<int, Data.GameLanguageData>();
    public Dictionary<int, Data.MissionData> MissionDataDic { get; private set; } = new Dictionary<int, Data.MissionData>();
    public Dictionary<int, Data.EvolutionData> EvolutionDataDic { get; private set; } = new Dictionary<int, Data.EvolutionData>();
    public Dictionary<int, Data.EvolutionItemData> EvolutionItemDataDic { get; private set; } = new Dictionary<int, Data.EvolutionItemData>();
    public Dictionary<int, Data.ErrorData> ErrorDataDic { get; private set; } = new Dictionary<int, Data.ErrorData>();
    public Dictionary<int, Data.MissionLanguageData> MissionLanguageDataDic { get; private set; } = new Dictionary<int, Data.MissionLanguageData>();
    public Dictionary<int, Data.GameSoundData> GameSoundDataDic { get; private set; } = new Dictionary<int, Data.GameSoundData>();
    public Dictionary<int, Data.CashItemData> CashItemDataDic { get; private set; } = new Dictionary<int, Data.CashItemData>();
    public Dictionary<int, Data.LoginRewardData> LoginRewardDataDic {get; private set;} = new Dictionary<int, LoginRewardData>();
    public void Init()
	{
        #region About IAP
        IapDic = LoadJson<Data.IapProductDataLoader, string, IapProductData>("IapProductData").MakeDict();
        #endregion
        TestDic = LoadJson<Data.TestDataLoader, int, Data.TestData>("TestData").MakeDict();
        TinyFarmDic = LoadJson<Data.TinyFarmDataLoader, int, Data.TinyFarmData>("TinyFarmEvent").MakeDict();
        EnemyDic = LoadJson<Data.EnemyDataLoader, int, Data.EnemyData>("EnemyData").MakeDict();
        PlayerDic = LoadJson<Data.PlayerDataLoader, int, Data.PlayerData>("PlayerData").MakeDict();
        CharacterItemSpriteDic = LoadJson<Data.CharacterItemSpriteDataLoader, int, Data.CharacterItemSpriteData>("CharacterItemSpriteData").MakeDict();
        SuberunkerItemDic = LoadJson<Data.SuberunkerItemDataLoader, int, Data.SuberunkerItemData>("SuberunkerItemData").MakeDict();
        SuberunkerItemSpriteDic = LoadJson<Data.SuberunkerItemSpriteDataLoader, int, Data.SuberunkerItemSpriteData>("SuberunkerItemSpriteData").MakeDict();
        DifficultySettingsDic = LoadJson<Data.DifficultySettingsDataLoader, int, Data.DifficultySettingsData>("DifficultySettingsData").MakeDict();
        ThoughtBubbleDataDic = LoadJson<Data.ThoughtBubbleDataLoader, int, Data.ThoughtBubbleData>("ThoughtBubbleData").MakeDict();
        ThoughtBubbleLanguageDataDic = LoadJson<Data.ThoughtBubbleLanguageDataLoader, int, Data.ThoughtBubbleLanguageData>("ThoughtBubbleLanguageData").MakeDict();
        GameLanguageDataDic = LoadJson<Data.GameLanguageDataLoader, int, Data.GameLanguageData>("GameLanguageData").MakeDict();
        MissionDataDic = LoadJson<Data.MissionDataLoader, int, Data.MissionData>("MissionData").MakeDict();
        EvolutionDataDic = LoadJson<Data.EvolutionDataLoader, int, Data.EvolutionData>("EvolutionData").MakeDict();
        EvolutionItemDataDic = LoadJson<Data.EvolutionItemDataLoader, int, Data.EvolutionItemData>("EvolutionItemData").MakeDict();
        ErrorDataDic = LoadJson<Data.ErrorDataLoader, int, Data.ErrorData>("ErrorData").MakeDict();
        MissionLanguageDataDic = LoadJson<Data.MissionLanguageDataLoader, int, Data.MissionLanguageData>("MissionLanguageData").MakeDict();
        GameSoundDataDic = LoadJson<Data.GameSoundDataLoader, int, Data.GameSoundData>("GameSoundData").MakeDict();
        CashItemDataDic = LoadJson<Data.CashItemDataLoader, int, Data.CashItemData>("CashItemData").MakeDict();
        LoginRewardDataDic = LoadJson<Data.LoginRewardDataLoader, int, Data.LoginRewardData>("LoginRewardData").MakeDict();
    }

    private Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
	{
		TextAsset textAsset = Managers.Resource.Load<TextAsset>(path);
        Debug.Log(textAsset.text);
		return JsonConvert.DeserializeObject<Loader>(textAsset.text);
	}
}
