using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class UI_LoadingPageTimeline : UI_Base
{
    PlayableDirector _playableDirector;
    Define.EScene _scene;
    private enum Sliders
    {
        Progress
    }
    private enum Texts
    {
        ProgressPercent,
    }
    private enum GameObjects
    {
        ProgressBar
    }
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindTexts(typeof(Texts));
        BindSliders(typeof(Sliders));
        BindObjects(typeof(GameObjects));
        _playableDirector = this.gameObject.GetOrAddComponent<PlayableDirector>();
        _playableDirector.playableAsset = this.gameObject.GetOrAddComponent<PlayableDirector>().playableAsset;
         
        return true;
    }
    public void LoadScene(Define.EScene type, string label = "")
    {
        Debug.Log("UI_LoadingPageTimeline");
        _scene = type;
        _playableDirector.Play();
        if(label == "")
        {
            GetObject((int)GameObjects.ProgressBar).SetActive(false);
            _playableDirector.stopped += OnPlayableDirectorStopped;
        }
        else
        {
            StartLoadAssets(label);
        }
    }
    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        Managers.Scene.LoadScene(_scene);
        Managers.UI.ClosePopupUI();
        //GameObject.Destroy(this.gameObject);
    }
    private void StartLoadAssets(string label)
    {
        Managers.Resource.LoadAllAsync<UnityEngine.Object>(label, (key, count, totalCount) =>
        {
            float progress = (float)count / totalCount;
            GetText((int)Texts.ProgressPercent).text = $"{count}/{totalCount}";
            GetSlider((int)Sliders.Progress).value = progress;
            if (count == totalCount)
            {
                _playableDirector.stopped += OnPlayableDirectorStopped;
                Managers.Data.Init();
            }
        });
    }
}
