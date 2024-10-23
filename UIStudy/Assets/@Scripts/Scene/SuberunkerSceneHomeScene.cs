using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using static Define;

public class SuberunkerSceneHomeScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        Managers.UI.ShowSceneUI<UI_SuberunkerSceneHomeScene>();

        Managers.Sound.Stop(ESound.Bgm);
        Managers.Sound.Play(Define.ESound.Bgm, "LobbyBGMSound", 0.2f);

        return true;
    }
}
