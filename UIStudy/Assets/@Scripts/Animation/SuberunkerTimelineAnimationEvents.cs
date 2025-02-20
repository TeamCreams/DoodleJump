using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuberunkerTimelineAnimationEvents : ObjectBase
{
    private CharacterController _characterController;

    private SpriteRenderer EyeSpriteRenderer;
    private SpriteRenderer EyebrowsSpriteRenderer;
    private SpriteRenderer HairSpriteRenderer;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        return true;
    }

    public override void SetInfo(int templateId)
    {
        _characterController = GetComponentInChildren<CharacterController>();
        Debug.Assert(_characterController != null, "is nullllllllllllllllll");
        EyeSpriteRenderer = Util.FindChild<SpriteRenderer>(go: _characterController.gameObject, name: "Eyes", recursive: true);
        EyebrowsSpriteRenderer = Util.FindChild<SpriteRenderer>(go: _characterController.gameObject, name: "Eyebrows", recursive: true);
        HairSpriteRenderer = Util.FindChild<SpriteRenderer>(go: _characterController.gameObject, name: "Hair", recursive: true);

        CommitPlayerCustomization();
    }

    public void CommitPlayerCustomization()
    {
        HairSpriteRenderer.sprite = Managers.Resource.Load<Sprite>($"{Managers.Game.ChracterStyleInfo.Hair}.sprite");
        EyebrowsSpriteRenderer.sprite = Managers.Resource.Load<Sprite>($"{Managers.Game.ChracterStyleInfo.Eyebrows}.sprite");
        EyeSpriteRenderer.sprite = Managers.Resource.Load<Sprite>($"{Managers.Game.ChracterStyleInfo.Eyes}.sprite");
    }
}
