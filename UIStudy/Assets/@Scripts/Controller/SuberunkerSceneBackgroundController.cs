using System.Collections;
using UnityEngine;

public class SuberunkerSceneBackgroundController : ObjectBase
{
    private SpriteRenderer _spriteRenderer;
    private Color _defalt = new Color(255, 255, 255);
    private Color _stoneShower = new Color(255, 0, 0);

    float _duration = 1; 
    float _smoothness = 0.02f; 

    public override bool Init()
	{
		if (false == base.Init())
		{
            return false;
		}
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        //Managers.Event.AddEvent(Define.EEventType.StartStoneShower, OnEvent_StartStoneShower);
        //Managers.Event.AddEvent(Define.EEventType.StopStoneShower, OnEvent_StopStoneShower);
        return true;
	}
    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(Define.EEventType.IsStoneShower, OnEvent_StartStoneShower);
        Managers.Event.RemoveEvent(Define.EEventType.StopStoneShower, OnEvent_StopStoneShower);
    }
    private void OnEvent_StartStoneShower(Component sender, object param)
    {
        StartCoroutine(SetColor(_defalt, _stoneShower));
    }

    private void OnEvent_StopStoneShower(Component sender, object param)
    {
        StartCoroutine(SetColor(_stoneShower, _defalt));
    }
    IEnumerator SetColor(Color a, Color b)
    {
        float progress = 0;
        float increment = _smoothness/_duration;

        while(progress < 1)
        {
            _spriteRenderer.color = Color.Lerp(a, b, progress);
            progress += increment;
            yield return new WaitForSeconds(_smoothness);
        }
    }
}
