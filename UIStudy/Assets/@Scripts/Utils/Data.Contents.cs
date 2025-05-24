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
        public int Gold;
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
        public float StoneShardPercent;
        public int Benefit;
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


    [Serializable]
    public class GameLanguageData
    {
        public int Id;
        public string KrText;
        public string EnText;
    }

    [Serializable]
    public class GameLanguageDataLoader : ILoader<int, GameLanguageData>
    {
        public List<GameLanguageData> gameLanguageDatas = new List<GameLanguageData>();

        public Dictionary<int, GameLanguageData> MakeDict()
        {
            Dictionary<int, GameLanguageData> dict = new Dictionary<int, GameLanguageData>();
            foreach (GameLanguageData gameLanguageData in gameLanguageDatas)
                dict.Add(gameLanguageData.Id, gameLanguageData);

            return dict;
        }
    }

    [Serializable]
    public class MissionData
    {
        public int Id;
        public EMission Type;
        public int LanguageId;
        // public string Title;
        // public string Explanation;
        public int Compensation;
        public int	Level;
        public EMissionType	MissionType;
        public int	Param1;
        public string	Param2;
        public DateTime	StartDate;
        public DateTime	FinishDate;
        public List<int> PrevMissionId = new();
    }

    [Serializable]
    public class MissionDataLoader : ILoader<int, MissionData>
    {
        public List<MissionData> missionDatas = new List<MissionData>();

        public Dictionary<int, MissionData> MakeDict()
        {
            Dictionary<int, MissionData> dict = new Dictionary<int, MissionData>();
            foreach (MissionData missionData in missionDatas)
                dict.Add(missionData.Id, missionData);

            return dict;
        }
    }

    [Serializable]
    public class MissionLanguageData
    {
        public int Id;
        public string KrTitle;
        public string KrExplanation;
        public string EnTitle;
        public string EnExplanation;
    }

    [Serializable]
    public class MissionLanguageDataLoader : ILoader<int, MissionLanguageData>
    {
        public List<MissionLanguageData> missionLanguageDatas = new List<MissionLanguageData>();

        public Dictionary<int, MissionLanguageData> MakeDict()
        {
            Dictionary<int, MissionLanguageData> dict = new Dictionary<int, MissionLanguageData>();
            foreach (MissionLanguageData missionLanguageData in missionLanguageDatas)
                dict.Add(missionLanguageData.Id, missionLanguageData);

            return dict;
        }
    }

    public class EvolutionData
    {
        public int Id;
        public List<float> Stats = new();
        public int Gold;
        public EStat StatOption;
        public string ItemSprite;
        public int BuyCount;
        public int PrevEvolutionId;
    }

    [Serializable]
    public class EvolutionDataLoader : ILoader<int, EvolutionData>
    {
        public List<EvolutionData> evolutionDatas = new List<EvolutionData>();

        public Dictionary<int, EvolutionData> MakeDict()
        {
            Dictionary<int, EvolutionData> dict = new Dictionary<int, EvolutionData>();
            foreach (EvolutionData evolutionData in evolutionDatas)
                dict.Add(evolutionData.Id, evolutionData);

            return dict;
        }
    }

    public class EvolutionItemData
    {
        public int Id;
        public int	Level;
        public List<int> EvolutionDataId;
    }

    [Serializable]
    public class EvolutionItemDataLoader : ILoader<int, EvolutionItemData>
    {
        public List<EvolutionItemData> evolutionItemDatas = new List<EvolutionItemData>();

        public Dictionary<int, EvolutionItemData> MakeDict()
        {
            Dictionary<int, EvolutionItemData> dict = new Dictionary<int, EvolutionItemData>();
            foreach (EvolutionItemData evolutionItemData in evolutionItemDatas)
                dict.Add(evolutionItemData.Id, evolutionItemData);

            return dict;
        }
    }

    public class ErrorData
    {
        public int Id;
        public EErrorCode Type;
        public string TitleEn;
        public string NoticeEn;
        public string TitleKr;
        public string NoticeKr; 
    }

    [Serializable]
    public class ErrorDataLoader : ILoader<int, ErrorData>
    {
        public List<ErrorData> errorDatas = new List<ErrorData>();

        public Dictionary<int, ErrorData> MakeDict()
        {
            Dictionary<int, ErrorData> dict = new Dictionary<int, ErrorData>();
            foreach (ErrorData errorData in errorDatas)
                dict.Add(errorData.Id, errorData);

            return dict;
        }
    }

    public class GameSoundData
    {
        public int Id;
        public ESound Type;
        public string SoundName;
        public float DefaultVolume;
    }

    [Serializable]
    public class GameSoundDataLoader : ILoader<int, GameSoundData>
    {
        public List<GameSoundData> gameSoundDatas = new List<GameSoundData>();

        public Dictionary<int, GameSoundData> MakeDict()
        {
            Dictionary<int, GameSoundData> dict = new Dictionary<int, GameSoundData>();
            foreach (GameSoundData gameSoundData in gameSoundDatas)
                dict.Add(gameSoundData.Id, gameSoundData);

            return dict;
        }
    }

    public class CashItemData
    {
        public int Id;
        public int Gold;
        public int Price;
        public string GoldSprite;
    }
    [Serializable]
    public class CashItemDataLoader : ILoader<int, CashItemData>
    {
        public List<CashItemData> cashItemDatas = new List<CashItemData>();

        public Dictionary<int, CashItemData> MakeDict()
        {
            Dictionary<int, CashItemData> dict = new Dictionary<int, CashItemData>();
            foreach (CashItemData cashItemData in cashItemDatas)
                dict.Add(cashItemData.Id, cashItemData);

            return dict;
        }
    }

    public class LoginRewardData
    {
        public int Id;
        public int Reward;
        public float Percent;
    }
    [Serializable]
    public class LoginRewardDataLoader : ILoader<int, LoginRewardData>
    {
        public List<LoginRewardData> loginRewardDatas = new List<LoginRewardData>();

        public Dictionary<int, LoginRewardData> MakeDict()
        {
            Dictionary<int, LoginRewardData> dict = new Dictionary<int, LoginRewardData>();
            foreach (LoginRewardData loginRewardData in loginRewardDatas)
                dict.Add(loginRewardData.Id, loginRewardData);

            return dict;
        }
    }
}