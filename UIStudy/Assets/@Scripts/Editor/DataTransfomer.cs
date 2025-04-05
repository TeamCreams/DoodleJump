
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Data;
using System.Linq;
using System.Collections;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;

public class DataTransfomer : EditorWindow
{

	[MenuItem("Tools/ParseExcel %#K")]
	public static void ParseExcelDatas()
    {
        #region About IAP
        ParseExcelDataToJson<IapProductDataLoader, IapProductData>("IapProductData");
        #endregion
        ParseExcelDataToJson<TinyFarmDataLoader, TinyFarmData>("TinyFarmEvent");
        ParseExcelDataToJson<EnemyDataLoader, EnemyData>("EnemyData");
        ParseExcelDataToJson<PlayerDataLoader, PlayerData>("PlayerData");
        ParseExcelDataToJson<CharacterItemSpriteDataLoader, CharacterItemSpriteData>("CharacterItemSpriteData");
		ParseExcelDataToJson<SuberunkerItemDataLoader, SuberunkerItemData>("SuberunkerItemData");
        ParseExcelDataToJson<SuberunkerItemSpriteDataLoader, SuberunkerItemSpriteData>("SuberunkerItemSpriteData");
        ParseExcelDataToJson<DifficultySettingsDataLoader, DifficultySettingsData>("DifficultySettingsData");
        ParseExcelDataToJson<ThoughtBubbleDataLoader, ThoughtBubbleData>("ThoughtBubbleData");
        ParseExcelDataToJson<ThoughtBubbleLanguageDataLoader, ThoughtBubbleLanguageData>("ThoughtBubbleLanguageData");
        ParseExcelDataToJson<GameLanguageDataLoader, GameLanguageData>("GameLanguageData");
        ParseExcelDataToJson<MissionDataLoader, MissionData>("MissionData");
        ParseExcelDataToJson<EvolutionDataLoader, EvolutionData>("EvolutionData");
        ParseExcelDataToJson<EvolutionItemDataLoader, EvolutionItemData>("EvolutionItemData");
        ParseExcelDataToJson<ErrorDataLoader, ErrorData>("ErrorData");
        ParseExcelDataToJson<MissionLanguageDataLoader, MissionLanguageData>("MissionLanguageData");
		ParseExcelDataToJson<GameSoundDataLoader, GameSoundData>("GameSoundData");
        ParseExcelDataToJson<CashItemDataLoader, CashItemData>("CashItemData");
		ParseExcelDataToJson<LoginRewardDataLoader, LoginRewardData>("LoginRewardData");

		Debug.Log("Complete");
	}

	#region Helpers
	private static void ParseExcelDataToJson<Loader, LoaderData>(string filename) where Loader : new() where LoaderData : new()
	{
		Loader loader = new Loader();

		FieldInfo field = loader.GetType().GetFields()[0];
		field.SetValue(loader, ParseExcelDataToList<LoaderData>(filename));

		string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
		File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}.json", jsonStr);
		AssetDatabase.Refresh();
	}

    private static List<LoaderData> ParseExcelDataToList<LoaderData>(string filename) where LoaderData : new()
	{
		List<LoaderData> loaderDatas = new List<LoaderData>();
		string[] lines;
		using (FileStream fs = new FileStream($"{Application.dataPath}/@Resources/Data/ExcelData/{filename}.csv", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
		{
			using (StreamReader sr = new StreamReader(fs))
			{
				lines = sr.ReadToEnd().Split("\n");
				Debug.Log(String.Join("\n",  lines));
			}
		}



		for (int l = 1; l < lines.Length; l++)
		{
			string[] row = lines[l].Replace("\r", "").Split(',');
			if (row.Length == 0)
				continue;
			if (string.IsNullOrEmpty(row[0]))
				continue;


			LoaderData loaderData = new LoaderData();

			System.Reflection.FieldInfo[] fields = typeof(LoaderData).GetFields();

            if (filename.Contains("Iap"))
            {
				Debug.Log($"_______ {string.Join("|", fields.Select(f => f.Name))}");
                //Debug.Log($"##### {row[f]}");
                //Debug.Log($"#### {value}");
                //Debug.Log($"### {field.FieldType.Name}");
            }

            //public int Id;
            //public string Name;
            for (int f = 0; f < fields.Length; f++)
			{
				FieldInfo field = loaderData.GetType().GetField(fields[f].Name);
				Type type = field.FieldType;

				if (type.IsGenericType)
				{
					object value = ConvertList(row[f], type);
					field.SetValue(loaderData, value);
				}
				else
				{
					// float, string, int 
					object value = ConvertValue(row[f], field.FieldType);


                    field.SetValue(loaderData, value);
				}
			}

			loaderDatas.Add(loaderData);
		}

		return loaderDatas;
	}

	private static object ConvertValue(string value, Type type)
	{
		if (string.IsNullOrEmpty(value))
			return null;

		TypeConverter converter = TypeDescriptor.GetConverter(type);
		return converter.ConvertFromString(value);
	}

	private static object ConvertList(string value, Type type)
	{
		if (string.IsNullOrEmpty(value))
			return null;

		// Reflection
		Type valueType = type.GetGenericArguments()[0];
		Type genericListType = typeof(List<>).MakeGenericType(valueType);
		var genericList = Activator.CreateInstance(genericListType) as IList;

		// Parse Excel
		var list = value.Split('&').Select(x => ConvertValue(x, valueType)).ToList();

		foreach (var item in list)
			genericList.Add(item);

		return genericList;
	}
	#endregion
}
#endif