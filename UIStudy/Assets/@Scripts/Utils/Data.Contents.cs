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
    public class EntityData
    {
        public int Id;
        public string Name;
        public int Speed;
        public int Life;
    }

    [Serializable]
    public class EntityDataLoader : ILoader<int, EntityData>
    {
        public List<EntityData> entityDatas = new List<EntityData>();

        public Dictionary<int, EntityData> MakeDict()
        {
            Dictionary<int, EntityData> dict = new Dictionary<int, EntityData>();
            foreach (EntityData entityData in entityDatas)
                dict.Add(entityData.Id, entityData);

            return dict;
        }
    }
}