﻿using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_DevLoadingScene : UI_Scene
{
	enum Texts
	{
		LoadingText
	}

	enum GameObjects
	{
		SceneList
	}

	enum Buttons
	{
		TinyFarmButton,
        SuberunkerButton
    }

	protected override void Init()
	{
		base.Init();

		BindButtons(typeof(Buttons));
		BindTexts(typeof(Texts));
		BindObjects(typeof(GameObjects));

		GetText((int)Texts.LoadingText).text = "로딩중...";
		GetObject((int)GameObjects.SceneList).SetActive(false);

		GetButton((int)Buttons.TinyFarmButton).gameObject.BindEvent(OnClick_TinyFarmScene, Define.EUIEvent.Click);
        GetButton((int)Buttons.SuberunkerButton).gameObject.BindEvent(OnClick_SuberunkerScene, Define.EUIEvent.Click);


        StartLoadAssets("PreLoad");
	}

	private void OnClick_TinyFarmScene(PointerEventData eventData)
	{
		Managers.Scene.LoadScene(Define.EScene.TinyFarmScene);
	}
    private void OnClick_SuberunkerScene(PointerEventData eventData)
    {
        Managers.Scene.LoadScene(Define.EScene.SuberunkerScene);
    }
    void StartLoadAssets(string label)
	{
		Managers.Resource.LoadAllAsync<UnityEngine.Object>(label, (key, count, totalCount) =>
		{
			Debug.Log($"{key} {count}/{totalCount}");

			if (count == totalCount)
			{
				Debug.Log("Load Complete");
				Managers.Data.Init();

				//Observable.Timer(new System.TimeSpan(0, 0, 5))
				//	.Subscribe(_ =>
				//	{
				//		if (label == "PreLoad")
				//		{
				//			GetText((int)Texts.LoadingText).text = "로딩완료...";
				//			GetObject((int)GameObjects.SceneList).SetActive(true);
				//		}
				//	});
				if (label == "PreLoad")
				{
					GetText((int)Texts.LoadingText).text = "로딩완료...";
					GetObject((int)GameObjects.SceneList).SetActive(true);
				}
			}
		});
	}
}