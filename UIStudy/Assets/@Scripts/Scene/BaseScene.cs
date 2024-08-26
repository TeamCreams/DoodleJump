using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseScene : InitBase
{
	public override bool Init()
	{
		if(base.Init() == false)
		{
			return false;
		}

		
		if (GameObject.FindObjectOfType<EventSystem>() == null)
		{
			var go = new GameObject("@EventSystem");
			go.GetOrAddComponent<EventSystem>();
			go.GetOrAddComponent<StandaloneInputModule>();

			DontDestroyOnLoad(go.gameObject);
		}

		return true;
	}


	public virtual void Clear()
	{
	}
}
