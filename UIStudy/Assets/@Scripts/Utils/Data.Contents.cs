using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int Speed;
        public int Life;
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
        public int Life;
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
    public class CharacterItemSpriteData
    {
        public int Id;
        public string SpriteName;
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

}