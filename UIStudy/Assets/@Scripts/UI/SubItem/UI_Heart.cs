using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Heart : UI_Base
{
	Image _image;

	protected override void Init()
	{
		base.Init();

		_image = this.GetComponent<Image>();
	}

	public void Show()
	{
		this.gameObject.SetActive(true);
	}

	public void Hide()
	{
		this.gameObject.SetActive(false);
	}

}
