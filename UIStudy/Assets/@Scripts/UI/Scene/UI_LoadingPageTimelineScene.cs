using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using static Define;


public class UI_LoadingPageTimelineScene : UI_Scene
{

    private enum Sliders
    {
        Progress,
        TotalProgress
    }
    private enum Texts
    {
        ProgressPercent,
        TotalProgressPercent
    }
    private enum GameObjects
    {
        ProgressBar
    }

    private PlayableDirector _playableDirector;
    public PlayableDirector PlayableDirector => _playableDirector;
    private Define.EScene _scene;

    private int _loadTotalCount = 0;
    private int _totalCount = 1;
    private AsyncOperation _loading = null;
    private LoadingPageTimelineScene _currentScene = null;


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
        _currentScene = Managers.Scene.CurrentScene as LoadingPageTimelineScene;
        return true;
    }

    public void SettingLoadScene()
    {
        //Debug.Log("YAAAAA In Event");
        _scene = Managers.Scene.NextScene;
        _playableDirector.Play();
        StartLoadAssets(Managers.Scene.Label);
    }

    // private void StartLoadAssets(object loadSceneParamlabel)
    // {
    //     throw new NotImplementedException();
    // }

    public void OnPlayableDirectorStopped(PlayableDirector director)
    {
        //_currentScene.Loading.allowSceneActivation = true;
        if (_currentScene != null && _currentScene.Loading != null)
        {
            _currentScene.Loading.allowSceneActivation = true;
        }
    }

    private void StartLoadAssets(string label)
    {
        //Debug.Log("YAAAAA In StartLoadAssets");

        if (!string.IsNullOrEmpty(label))
        {
            // label이 있는 경우 에셋 로드
            _loadTotalCount++;
            _totalCount++;
            Managers.Resource.LoadAllAsync<UnityEngine.Object>(label, (key, count, totalCount) =>
            {
                //Debug.Log("YAAAAA In LoadAllAsync");

                float progress = (float)count / totalCount;
                UpdateProgress(progress);

                if (count == totalCount)
                {
                    Managers.Data.Init();
                    StartCoroutine(LoadSceneCoroutine());
                }
                //UpdateTotalProgress();
            });
        }
        else
        {
            // label이 없는 경우 씬 로드
            StartCoroutine(LoadSceneCoroutine());
        }
    }
    private IEnumerator LoadSceneCoroutine()
    {
        _loading = SceneManager.LoadSceneAsync(_scene.ToString());
        _loading.allowSceneActivation = false;

        while (!_loading.isDone)
        {
            float progress = _loading.progress;
            UpdateProgress(progress);
            //UpdateTotalProgress();
            if (0.9f <= progress)
            {
                progress = 1;
                UpdateProgress(progress);
                break;
            }
            yield return null;
        }
        _loadTotalCount++;
        //UpdateTotalProgress();
        _playableDirector.stopped += OnPlayableDirectorStopped;
    }
    public void UpdateProgress(float progress)
    {
        GetText((int)Texts.ProgressPercent).text = $"{progress}";
        GetSlider((int)Sliders.Progress).value = progress;
    }
    public void UpdateTotalProgress(int cnt, int cnt2)
    {
        float progress = (float)cnt / cnt2 * 100;
        GetText((int)Texts.TotalProgressPercent).text = $"{progress}/{100}";
        GetSlider((int)Sliders.TotalProgress).value = progress / 100;
    }
}
