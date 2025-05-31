using System;
using System.Collections;
using System.Collections.Generic;
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

    private List<string> _labelList = new List<string>(); 
    private int _currentLabelIndex = 0;
    private float _currentLabelProgress = 0f;
    
    public override bool Init()
    {
        if(base.Init() == false)
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
        
        CheckAndSetupLabels();
        
        // 라벨이 있으면 로드, 없으면 바로 씬 로드
        if(0 < _labelList.Count)
        {
            // 총 로드 항목 수 계산 (라벨 수 + 씬 로드)
            _totalCount = _labelList.Count + 1;
            StartLoadNextLabel();
        }
        else
        {
            StartCoroutine(LoadSceneCoroutine());
        }
    }
    private void CheckAndSetupLabels()
    {
        _labelList.Clear();
        _currentLabelIndex = 0;
        _loadTotalCount = 0;

        if(Managers.Scene.Labels != null && Managers.Scene.Labels.Count > 0)
        {
            foreach(string label in Managers.Scene.Labels)
            {
                if(!string.IsNullOrEmpty(label))
                {
                    _labelList.Add(label);
                }
            }
        }
    }

    private void StartLoadNextLabel()
    {
        if(_currentLabelIndex < _labelList.Count)
        {
            string currentLabel = _labelList[_currentLabelIndex];
            StartLoadAssets(currentLabel);
        }
        else
        {
            Managers.Data.Init();
            StartCoroutine(LoadSceneCoroutine());
        }
    }

    private void StartLoadAssets(string label)
    {
        if(!string.IsNullOrEmpty(label))
        {
            _currentLabelProgress = 0f;
            
            Managers.Resource.LoadAllAsync<UnityEngine.Object>(label, (key, count, totalCount) =>
            {
                _currentLabelProgress = (float)count/totalCount;
                
                UpdateTotalProgress();
                
                if(count == totalCount)
                {
                    // 현재 라벨 로드 완료
                    _loadTotalCount++;
                    _currentLabelIndex++;
                    
                    // 다음 라벨 로드 시작
                    StartLoadNextLabel();
                }
            });
        }
    }
    
    private void UpdateTotalProgress()
    {
        float currentTaskProgress = _currentLabelProgress; 
        
        float totalProgress = 0f;
        
        if(0 < _totalCount)
        {
            totalProgress = (float)_loadTotalCount/_totalCount;
            
            if(_currentLabelIndex < _labelList.Count || _loading != null)
            {
                float currentTaskContribution = currentTaskProgress/_totalCount;
                totalProgress += currentTaskContribution;
            }
        }
        
        // UI 업데이트
        _ui.UpdateProgress(currentTaskProgress);
        _ui.UpdateTotalProgress(_loadTotalCount, _totalCount);
    }
    
    private IEnumerator LoadSceneCoroutine()
    {
        _loading = SceneManager.LoadSceneAsync(_scene.ToString());
        _loading.allowSceneActivation = false;
        
        while(!_loading.isDone)
        {
            float progress = _loading.progress;
            
            _currentLabelProgress = progress >= 0.9f ? 1.0f : progress / 0.9f;
            
            UpdateTotalProgress();
            
            if(0.9f <= progress)
            {
                _currentLabelProgress = 1.0f;
                UpdateTotalProgress();
                _loading.allowSceneActivation = true;
                break;
            }
            yield return null;
        }
        
        _loadTotalCount++;
        UpdateTotalProgress();
    }
}
