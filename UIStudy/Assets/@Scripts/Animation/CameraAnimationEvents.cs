using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimationEvents : ObjectBase
{
    Camera _camera;

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
        _camera = GetComponent<Camera>();

        return true;
    }

    public void TestCameraShake()
    {
        StartCoroutine(ShakeCo(1.0f, 0.2f));
    }

    IEnumerator ShakeCo(float shakePower, float shakeDuration)
    {
        Vector3 cameraPos = Camera.main.transform.position;
        float timer = 0.0f;
        while (timer < shakeDuration)
        {
            float x = Random.Range(-1.0f, 1.0f);
            float y = Random.Range(-1.0f, 1.0f);

            x *= shakePower;
            y *= shakePower;

            Vector3 newCameraPos = cameraPos + new Vector3(x, y, 0);
            Camera.main.transform.position = newCameraPos;

            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        yield return null;
    }

    public override void SetInfo(int templateId)
    {
        Debug.Log("is SetttinggQ!!!!!!!");
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
