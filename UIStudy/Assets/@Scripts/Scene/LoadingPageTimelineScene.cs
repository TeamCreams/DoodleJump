using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using static Define;


public class LoadingPageTimelineScene : BaseScene
{
    private UI_LoadingPageTimelineScene _ui;
    private int _loadTotalCount = 0;
    private int _totalCount = 1; // 로드씬 기본값
    private AsyncOperation _loading = null;
    public AsyncOperation Loading => _loading;
    private Define.EScene _scene;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
 
        _ui = Managers.UI.ShowSceneUI<UI_LoadingPageTimelineScene>();
        return true;
    }
    void Start() // 이거 고쳐야함?
    {
         LoadScene();
    }
    public void LoadScene()
    {
        SettingLoadScene();
    }
    
    public void SettingLoadScene()
    {
        _scene = Managers.Scene.NextScene;
        _ui.PlayableDirector.Play();
        StartLoadAssets(Managers.Scene.Label);
    }

    private void StartLoadAssets(string label)
    {
        if (!string.IsNullOrEmpty(label))
        {
            // label이 있는 경우 에셋 로드
            _loadTotalCount ++;
            _totalCount ++;
            Managers.Resource.LoadAllAsync<UnityEngine.Object>(label, (key, count, totalCount) =>
            {
                float progress = (float)count / totalCount;
                _ui.UpdateProgress(progress);

                if (count == totalCount)
                {
                    Managers.Data.Init();
                    StartCoroutine(LoadSceneCoroutine());
                }
                _ui.UpdateTotalProgress(_loadTotalCount, _totalCount);
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
            _ui.UpdateProgress(progress);
            _ui.UpdateTotalProgress(_loadTotalCount, _totalCount);
            if(0.9f <= progress)
            {
                progress = 1;
                _ui.UpdateProgress(progress);
                break;
            }
            yield return null;
        }
        _loadTotalCount++;
        _ui.UpdateTotalProgress(_loadTotalCount, _totalCount);
        _ui.PlayableDirector.stopped += _ui.OnPlayableDirectorStopped;
    }
}
