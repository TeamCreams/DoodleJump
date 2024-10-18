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

    public void Init()
	{
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

    }

    private Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
	{
		TextAsset textAsset = Managers.Resource.Load<TextAsset>(path);
		Debug.Log(textAsset.text);
		return JsonConvert.DeserializeObject<Loader>(textAsset.text);
	}
}
