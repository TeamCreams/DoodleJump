using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

public interface ILoader<Key, Value>
{
	Dictionary<Key, Value> MakeDict();
}
public class DataManager
{
	public Dictionary<int, Data.TestData> TestDic { get; private set; } = new Dictionary<int, Data.TestData>();
    public Dictionary<int, Data.TinyFarmData> TinyFarmDic { get; private set; } = new Dictionary<int, Data.TinyFarmData>();


    public void Init()
	{
		TestDic = LoadJson<Data.TestDataLoader, int, Data.TestData>("TestData").MakeDict();
        TinyFarmDic = LoadJson<Data.TinyFarmDataLoader, int, Data.TinyFarmData>("TinyFarmEvent").MakeDict();
    }

	private Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
	{
		TextAsset textAsset = Managers.Resource.Load<TextAsset>(path);
		Debug.Log(textAsset.text);
		return JsonConvert.DeserializeObject<Loader>(textAsset.text);
	}
}
