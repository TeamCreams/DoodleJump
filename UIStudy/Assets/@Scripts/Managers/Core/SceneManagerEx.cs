using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
	public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
	private Define.EScene _nextScene;
	public Define.EScene NextScene => _nextScene;

	private string _label = "";
	public string Label => _label;
	private List<string> _labels = new List<string>();
	public IReadOnlyList<string> Labels => _labels;
	public void LoadScene(Define.EScene type)
	{
		Managers.Clear();
		SceneManager.LoadScene(GetSceneName(type));
		//Managers.InitScene();
	}

	public void LoadSceneWithProgress(Define.EScene type, string label = "")
	{
		Managers.Clear();
		_nextScene = type;
		_label = label;
		SceneManager.LoadScene(GetSceneName(Define.EScene.LoadingPageTimelineScene));
    }
	public void LoadSceneWithProgress(Define.EScene type, List<string> labels)
	{
		Managers.Clear();
		_nextScene = type;
		_labels = labels;
		SceneManager.LoadScene(GetSceneName(Define.EScene.LoadingPageTimelineScene));
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
