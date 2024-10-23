using System;
using static Define;
using UnityEngine;

public class ChooseCharacterScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        Managers.UI.ShowSceneUI<UI_ChooseCharacterScene>();

        Managers.Sound.Stop(ESound.Bgm);
        Managers.Sound.Play(Define.ESound.Bgm, "LobbyBGMSound", 0.6f);

        return true;
    }
}

