using GameApi.Dtos;
using UnityEngine;
using WebApi.Models.Dto;
using static Define;

public class SuberunkerSceneHomeScene : BaseScene
{
    private System.Random _random = new System.Random();

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        Managers.UI.ShowSceneUI<UI_SuberunkerSceneHomeScene>();

        var inputObject = new GameObject("@Input_Scene");
        var inputScene = inputObject.GetOrAddComponent<Input_SuberunkerSceneHomeScene>();

        Managers.Input.KeyAction -= inputScene.OnKeyAction;
        Managers.Input.KeyAction += inputScene.OnKeyAction;

        Managers.Sound.Stop(ESound.Bgm);
        Managers.Sound.Play(ESound.Bgm, "LobbyBGMSound", 0.2f);
        Managers.Event.AddEvent(EEventType.UpdateEnergy, OnEvent_UpdateEnergy);
        return true;
    }

    void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.UpdateEnergy, OnEvent_UpdateEnergy);
    }

    public int GetRandomReward()
    {
        double totalPercent = 0;
        foreach (var reward in Managers.Data.LoginRewardDataDic.Values)
        {
            totalPercent += reward.Percent;
        }

        double roll = _random.NextDouble() * totalPercent;
        double cumulative = 0;

        foreach (var reward in Managers.Data.LoginRewardDataDic.Values)
        {
            cumulative += reward.Percent;
            if (roll <= cumulative)
            {
                return Managers.Data.LoginRewardDataDic[reward.Id].Reward;
            }
        }

        return Managers.Data.LoginRewardDataDic[0].Reward;
    }

    public void OnEvent_UpdateEnergy(Component sender, object param)
    {
        Debug.Log("OnEvent_UpdateEnergy!!!"); 

        Managers.WebContents.UpdateEnergy(new ReqDtoUpdateEnergy()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId
        },
        (response) =>
        {
            Debug.Log("OnEvent_UpdateEnergy" + Managers.Game.UserInfo.LatelyEnergy);
            Managers.Game.UserInfo.Energy = response.Energy;
            Managers.Game.UserInfo.LatelyEnergy = response.LatelyEnergy;
            Managers.Event.TriggerEvent(EEventType.UIRefresh);
        },
        (errorCode) =>
        {
            UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_NetworkSaveError));
        }
        );
    }
    
}
