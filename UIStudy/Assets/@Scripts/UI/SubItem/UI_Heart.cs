using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Heart : UI_Base
{
	Image _image;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        _image = this.GetComponent<Image>();
		return true;
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
