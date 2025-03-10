﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
	public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

	public void LoadScene(Define.EScene type)
	{
		Managers.Clear();
		SceneManager.LoadScene(GetSceneName(type));
        //Managers.InitScene();
    }

	private string GetSceneName(Define.EScene type)
	{
		string name = System.Enum.GetName(typeof(Define.EScene), type);
		Debug.Log($"GET SCENE : {name}");
		return name;
	}

	public void Clear()
	{
		CurrentScene.Clear();
	}
}
