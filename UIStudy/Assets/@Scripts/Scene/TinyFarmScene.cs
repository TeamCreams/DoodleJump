using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TinyFarmScene : BaseScene
{
	public override bool Init()
	{
		if(base.Init() == false)
		{ 
			return false;
		}

		Managers.UI.ShowSceneUI<UI_TinyFarmScene>();


		Managers.Input.KeyAction -= KeyActionEvent;
		Managers.Input.KeyAction += KeyActionEvent;

		return true;
	}

	void KeyActionEvent()
	{
	}


}
