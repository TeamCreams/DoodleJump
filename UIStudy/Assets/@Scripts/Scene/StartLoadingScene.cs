using UnityEngine;
using UnityEngine.Playables;

public class StartLoadingScene : BaseScene
{
    PlayableDirector _playableDirector;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        UI_StartLoadingScene scene = Managers.UI.ShowSceneUI<UI_StartLoadingScene>();
        _playableDirector = scene.gameObject.GetOrAddComponent<PlayableDirector>();
        _playableDirector.name = "UI_StartLoadingScene";
        _playableDirector.Play();
        return true;
    }
}
