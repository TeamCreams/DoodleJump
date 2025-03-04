using UnityEngine;
using static Define;

public class UI_CharacterStyleItem : UI_Base
{
    enum Images
    {
       PrevItem_Image,
       NextItem_Image
    }

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindImages(typeof(Images));
        return true;
    }

    public void SetInfo(EEquipType style)
    {
        switch(style)
        {
            case EEquipType.Hair:
            {
                GetImage((int)Images.PrevItem_Image).sprite = Managers.Resource.Load<Sprite>($"{Managers.Game.ChracterStyleInfo.Hair}Icon.sprite");
                GetImage((int)Images.NextItem_Image).sprite = Managers.Resource.Load<Sprite>($"{Managers.Game.ChracterStyleInfo.TempHair}Icon.sprite");
            }
            break;
            case EEquipType.Eyes:
            {
                GetImage((int)Images.PrevItem_Image).sprite = Managers.Resource.Load<Sprite>($"{Managers.Game.ChracterStyleInfo.Eyes}Icon.sprite");
                GetImage((int)Images.NextItem_Image).sprite = Managers.Resource.Load<Sprite>($"{Managers.Game.ChracterStyleInfo.TempEyes}Icon.sprite");
            }
            break;
            case EEquipType.Eyebrows:
            {
                GetImage((int)Images.PrevItem_Image).sprite = Managers.Resource.Load<Sprite>($"{Managers.Game.ChracterStyleInfo.Eyebrows}Icon.sprite");
                GetImage((int)Images.NextItem_Image).sprite = Managers.Resource.Load<Sprite>($"{Managers.Game.ChracterStyleInfo.TempEyebrows}Icon.sprite");
            }
            break;
        }
    }
}
