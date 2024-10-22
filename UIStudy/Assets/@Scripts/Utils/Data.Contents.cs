using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Define;

namespace Data
{
	[Serializable]
	public class TestData
	{
		public int Id;
		public string Name;
	}

	[Serializable]
	public class TestDataLoader : ILoader<int, TestData>
	{
		public List<TestData> tests = new List<TestData>();

		public Dictionary<int, TestData> MakeDict()
		{
			Dictionary<int, TestData> dict = new Dictionary<int, TestData>();
			foreach (TestData testData in tests)
				dict.Add(testData.Id, testData);

			return dict;
		}
	}
	#region Creature
    [Serializable]
    public class CreatureInfoData
    {
        public int DataID;
        public string Remark;
        // Stat
        public float Atk;
        public float Def;
        public float MaxHp;
        public float Recovery;
        public float CritRate;
        public float AttackRange;
        public float AttackDelay;
        public float AttackDelayReduceRate;
        public float DodgeRate;
        public float SkillCooldownReduceRate;
        public float MoveSpeed;
        public float ElementAdvantageRate;
        public float GoldAmountAdvantageRate;
        public float ExpAmountAdvantageRate;
        public float BossAtkAdvantageRate;
        public float ActiveSKillAdvantageRate;
        public float Luck;
    }

    [Serializable]
    public class CreatureInfoDataLoader : ILoader<int, CreatureInfoData>
    {
        public List<CreatureInfoData> CreatureInfoDataList = new List<CreatureInfoData>();

        public Dictionary<int, CreatureInfoData> MakeDict()
        {
            Dictionary<int, CreatureInfoData> dict = new Dictionary<int, CreatureInfoData>();
            foreach (CreatureInfoData infoData in CreatureInfoDataList)
            {
                dict.Add(infoData.DataID, infoData);
            }

            return dict;
        }
    }
	#endregion

    [Serializable]
    public class TinyFarmData
    {
        public int Id;
        public string EventName;
        public string EventDetails;
        public int Compensation1;
        public int Compensation2;
        public int Event;
    }

    [Serializable]
    public class TinyFarmDataLoader : ILoader<int, TinyFarmData>
    {
        public List<TinyFarmData> tinyFarmDatas = new List<TinyFarmData>();

        public Dictionary<int, TinyFarmData> MakeDict()
        {
            Dictionary<int, TinyFarmData> dict = new Dictionary<int, TinyFarmData>();
            foreach (TinyFarmData tinyFarmData in tinyFarmDatas)
                dict.Add(tinyFarmData.Id, tinyFarmData);

            return dict;
        }
    }

    [Serializable]
    public class EnemyData
    {
        public int Id;
        public string Name;
        public string SpriteName;
        public int Speed;
        public float Damage;
    }

    [Serializable]
    public class EnemyDataLoader : ILoader<int, EnemyData>
    {
        public List<EnemyData> enemyDatas = new List<EnemyData>();

        public Dictionary<int, EnemyData> MakeDict()
        {
            Dictionary<int, EnemyData> dict = new Dictionary<int, EnemyData>();
            foreach (EnemyData enemyData in enemyDatas)
                dict.Add(enemyData.Id, enemyData);

            return dict;
        }
    }

    [Serializable]
    public class PlayerData
    {
        public int Id;
        public string Name;
        public float Speed;
        public float Hp;
        public float Luck;

        public PlayerData()
        {

        }

        public PlayerData(PlayerData original)
        {
            Id = original.Id;
            Name = original.Name;
            Speed = original.Speed;
            Hp = original.Hp;
            Luck = original.Luck;
        }
    }

    [Serializable]
    public class PlayerDataLoader : ILoader<int, PlayerData>
    {
        public List<PlayerData> playerDatas = new List<PlayerData>();

        public Dictionary<int, PlayerData> MakeDict()
        {
            Dictionary<int, PlayerData> dict = new Dictionary<int, PlayerData>();
            foreach (PlayerData playerData in playerDatas)
                dict.Add(playerData.Id, playerData);

            return dict;
        }
    }

    [Serializable]
    public class SuberunkerItemData
    {
        public int Id;
        public string Name;
        public float Chance;
        public EStat Option1;
        public EStatModifierType Option1ModifierType;
        public float Option1Param;
        public EStat Option2;
        public EStatModifierType Option2ModifierType;
        public float Option2Param;
        public EStat Option3;
        public EStatModifierType Option3ModifierType;
        public float Option3Param;
        public EStat Option4;
        public EStatModifierType Option4ModifierType;
        public float Option4Param;
        public float AddHp;
        public float Duration;
    }


    [Serializable]
    public class SuberunkerItemDataLoader : ILoader<int, SuberunkerItemData>
    {
        public List<SuberunkerItemData> suberunkerItemDatas = new List<SuberunkerItemData>();

        public Dictionary<int, SuberunkerItemData> MakeDict()
        {
            Dictionary<int, SuberunkerItemData> dict = new Dictionary<int, SuberunkerItemData>();
            foreach (SuberunkerItemData suberunkerItemData in suberunkerItemDatas)
                dict.Add(suberunkerItemData.Id, suberunkerItemData);

            return dict;
        }
    }

    [Serializable]
    public class CharacterItemSpriteData
    {
        public int Id;
        public string SpriteName;
        public EEquipType EquipType;
    }

    [Serializable]
    public class CharacterItemSpriteDataLoader : ILoader<int, CharacterItemSpriteData>
    {
        public List<CharacterItemSpriteData> characterItemSpriteDatas = new List<CharacterItemSpriteData>();

        public Dictionary<int, CharacterItemSpriteData> MakeDict()
        {
            Dictionary<int, CharacterItemSpriteData> dict = new Dictionary<int, CharacterItemSpriteData>();
            foreach (CharacterItemSpriteData characterItemSpriteData in characterItemSpriteDatas)
                dict.Add(characterItemSpriteData.Id, characterItemSpriteData);

            return dict;
        }
    }

    [Serializable]
    public class SuberunkerItemSpriteData
    {
        public int Id;
        public string Name;
        public EStat StatOption;
    }

    [Serializable]
    public class SuberunkerItemSpriteDataLoader : ILoader<int, SuberunkerItemSpriteData>
    {
        public List<SuberunkerItemSpriteData> suberunkerItemSpriteDatas = new List<SuberunkerItemSpriteData>();

        public Dictionary<int, SuberunkerItemSpriteData> MakeDict()
        {
            Dictionary<int, SuberunkerItemSpriteData> dict = new Dictionary<int, SuberunkerItemSpriteData>();
            foreach (SuberunkerItemSpriteData suberunkerItemSpriteData in suberunkerItemSpriteDatas)
                dict.Add(suberunkerItemSpriteData.Id, suberunkerItemSpriteData);

            return dict;
        }
    }

    [Serializable]
    public class DifficultySettingsData
    {
        public int Id;
        public int Level;
        public int ChallengeScale;  // 데이터로만 게임 세팅할수있게끔.
        public float StoneGenerateStartTime;
        public float StoneGenerateFinishTime;
        //public float StoneShowerStartTime;
        //public float StoneShowerFinishTime;
        public float StoneShowerPeriodStartTime;
        public float StoneShowerPeriodFinishTime;
    }

    [Serializable]
    public class DifficultySettingsDataLoader : ILoader<int, DifficultySettingsData>
    {
        public List<DifficultySettingsData> difficultySettingsDatas = new List<DifficultySettingsData>();

        public Dictionary<int, DifficultySettingsData> MakeDict()
        {
            Dictionary<int, DifficultySettingsData> dict = new Dictionary<int, DifficultySettingsData>();
            foreach (DifficultySettingsData difficultySettingsData in difficultySettingsDatas)
                dict.Add(difficultySettingsData.Id, difficultySettingsData);

            return dict;
        }
    }

    [Serializable]
    public class ThoughtBubbleData
    {
        public int Id;
        public EBehavior Behavior;
        public string TextId;
    }

    [Serializable]
    public class ThoughtBubbleDataLoader : ILoader<int, ThoughtBubbleData>
    {
        public List<ThoughtBubbleData> thoughtBubbleDatas = new List<ThoughtBubbleData>();

        public Dictionary<int, ThoughtBubbleData> MakeDict()
        {
            Dictionary<int, ThoughtBubbleData> dict = new Dictionary<int, ThoughtBubbleData>();
            foreach (ThoughtBubbleData thoughtBubbleData in thoughtBubbleDatas)
                dict.Add(thoughtBubbleData.Id, thoughtBubbleData);

            return dict;
        }
    }

    [Serializable]
    public class ThoughtBubbleLanguageData
    {
        public int Id;
        public string TextId;
        public string KrText;
        public string EnText;
    }

    [Serializable]
    public class ThoughtBubbleLanguageDataLoader : ILoader<int, ThoughtBubbleLanguageData>
    {
        public List<ThoughtBubbleLanguageData> thoughtBubbleLanguageDatas = new List<ThoughtBubbleLanguageData>();

        public Dictionary<int, ThoughtBubbleLanguageData> MakeDict()
        {
            Dictionary<int, ThoughtBubbleLanguageData> dict = new Dictionary<int, ThoughtBubbleLanguageData>();
            foreach (ThoughtBubbleLanguageData thoughtBubbleLanguageData in thoughtBubbleLanguageDatas)
                dict.Add(thoughtBubbleLanguageData.Id, thoughtBubbleLanguageData);

            return dict;
        }
    }
}