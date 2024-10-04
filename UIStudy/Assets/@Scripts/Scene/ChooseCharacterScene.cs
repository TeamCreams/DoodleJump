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

        AudioClip lobbyBGMAudio = Managers.Resource.Load<AudioClip>("LobbyBGMSound");
        Managers.Sound.Play(Define.ESound.Bgm, lobbyBGMAudio, 0.6f);

        return true;
    }
}

