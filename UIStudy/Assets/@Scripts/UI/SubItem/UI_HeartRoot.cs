using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using static Define;

public class UI_HeartRoot : UI_Base
{
	HorizontalLayoutGroup _horizontalLayoutGroup;

	List<UI_Heart> _heartList = new List<UI_Heart>();
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        _horizontalLayoutGroup = this.GetComponent<HorizontalLayoutGroup>();
		Managers.Event.AddEvent(EEventType.GetLife, OnEvent_AddHeart);
		return true;
	}

	public void SetLife(int life)
	{
		_heartList.Clear();
		foreach (Transform child in _horizontalLayoutGroup.transform)
		{
			Managers.Resource.Destroy(child.gameObject);
		}

		for (int i = 0; i < life; i++)
		{
			var heart = Managers.UI.MakeSubItem<UI_Heart>(parent: _horizontalLayoutGroup.transform);
			_heartList.Add(heart);
		}
	}

	public void RemoveHeart(int removeCount = 1)
	{
		if (_heartList.Count == 0) return;

		var minValue = Mathf.Min(_heartList.Count, removeCount);

		for (int i = 0; i < minValue; i++)
		{
			var lastHeart = _heartList.Last();
			_heartList.Remove(lastHeart);
			Managers.Resource.Destroy(lastHeart.gameObject);
		}
	}

	public void OnEvent_AddHeart(Component sender, object param)
    {
        AudioClip addAudio = Managers.Resource.Load<AudioClip>("AddLifeSound");
        Managers.Sound.Play(ESound.Effect, addAudio);
        var heart = Managers.UI.MakeSubItem<UI_Heart>(parent: _horizontalLayoutGroup.transform);
        _heartList.Add(heart);
    }

    public void AddHeart(int addCount = 1)
	{
		for (int i = 0; i < addCount; i++)
		{
			var heart = Managers.UI.MakeSubItem<UI_Heart>(parent: _horizontalLayoutGroup.transform);
			_heartList.Add(heart);
		}
	}
}
