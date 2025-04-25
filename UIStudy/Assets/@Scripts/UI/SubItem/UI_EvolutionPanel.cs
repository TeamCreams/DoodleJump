using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_EvolutionPanel : UI_Base
{
    private enum GameObjects
    {
        EvolutionRoot,
    }
    
    // ScrollRect 컴포넌트 참조를 위한 변수 추가
    private ScrollRect _scrollRect;
    private Transform _evolutionRoot = null;
    private List<GameObject> _itemList = new List<GameObject>();
    private Coroutine _scrollCoroutine; // 스크롤 코루틴 참조

    public override bool Init()  // 이 함수가 Awake()로 사용됨
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindObjects(typeof(GameObjects));
        _evolutionRoot = GetObject((int)GameObjects.EvolutionRoot).transform;
        
        // ScrollRect 컴포넌트 찾기 (Evolution Panel 하위에 있다고 가정)
        _scrollRect = GetComponentInChildren<ScrollRect>();
        
        Managers.Event.AddEvent(EEventType.Evolution, SetInventoryItems);

        return true;
    }
    
    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.Evolution, SetInventoryItems);
    }
    
    // 패널이 활성화될 때마다 호출되는 함수
    private void OnEnable()
    {
        // 아이템이 세팅된 후 스크롤 위치 조정
        StartCoroutine(DelayedScrollToTarget());
    }
    
    private void SetInventoryItems(Component sender = null, object param = null)
    {
        AllPush();
        var list = Managers.Data.EvolutionItemDataDic;
        foreach (var item in list)
        { 
            SpawnItem(item.Value.Id);
        }
        
        // 아이템 생성 직후 스크롤 위치 조정
        ScrollToCurrentEvolutionLevel();
    }

    private void AllPush()
    {
        foreach(var _item in _itemList)
        {
            Managers.Resource.Destroy(_item.gameObject);
        }
        _itemList.Clear();
    }

    private void SpawnItem(int id)
    {
        var item = Managers.UI.MakeSubItem<UI_EvolutionItemSet>(parent: _evolutionRoot, pooling: true);
        item.SetInfo(id);
        _itemList.Add(item.gameObject);
    }
    
    /// <summary>
    /// 현재 진화 레벨에 해당하는 아이템 위치로 스크롤
    /// </summary>
    private void ScrollToCurrentEvolutionLevel()
    {
        if (_scrollRect == null) return;
        
        // 이전 스크롤 코루틴이 있다면 중지
        if (_scrollCoroutine != null)
        {
            StopCoroutine(_scrollCoroutine);
        }
        
        // 새로운 스크롤 코루틴 시작
        _scrollCoroutine = StartCoroutine(ScrollToTargetCoroutine());
    }
    
    /// <summary>
    /// 아이템 생성 후 프레임을 기다린 다음 스크롤 (레이아웃 업데이트 대기)
    /// </summary>
    private IEnumerator DelayedScrollToTarget()
    {
        // 한 프레임 대기 (레이아웃 업데이트를 위해)
        yield return null;
        
        // 스크롤 실행
        ScrollToCurrentEvolutionLevel();
    }
    
    /// <summary>
    /// 목표 위치로 부드럽게 스크롤하는 코루틴
    /// </summary>
    /// <summary>
/// 목표 위치로 부드럽게 스크롤하는 코루틴
/// </summary>
private IEnumerator ScrollToTargetCoroutine()
{
    // 한 프레임 더 대기 (레이아웃이 완전히 업데이트되도록)
    yield return null;
    
    // 현재 유저의 진화 레벨 (진화 세트 레벨)
    int currentLevel = Managers.Game.UserInfo.EvolutionSetLevel;
    
    // 아이템이 역순으로 정렬되어 있으므로 인덱스 계산을 변경
    // 총 아이템 개수 - 현재 레벨 - 1
    int totalItems = _scrollRect.content.childCount;
    int targetIndex = totalItems - currentLevel - 1;
    
    // 만약 현재 레벨이 3이고 구매해야 할 것이 4레벨이라면
    // targetIndex는 총 개수에서 4를 뺀 위치가 됨
    
    // Content의 RectTransform
    RectTransform content = _scrollRect.content;
    
    // 아이템들이 있는 부모 오브젝트에서 자식들 가져오기
    if (targetIndex >= 0 && targetIndex < content.childCount)
    {
        // 목표 아이템의 RectTransform
        RectTransform targetItem = content.GetChild(targetIndex).GetComponent<RectTransform>();
        
        // 목표 위치 계산 (세로 스크롤 기준)
        float targetPosition = CalculateTargetPosition(targetItem, content);
        
        // 현재 위치
        float startPosition = _scrollRect.verticalNormalizedPosition;
        
        // 0.3초 동안 부드럽게 스크롤
        float elapsedTime = 0f;
        float duration = 0.3f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            
            // 부드러운 보간을 위한 SmoothStep 함수 사용
            float smoothT = t * t * (3f - 2f * t);
            
            // 새로운 위치로 설정
            float newPosition = Mathf.Lerp(startPosition, targetPosition, smoothT);
            _scrollRect.verticalNormalizedPosition = newPosition;
            
            yield return null;
        }
        
        // 최종 위치로 정확히 설정
        _scrollRect.verticalNormalizedPosition = targetPosition;
    }
}
    
    /// <summary>
    /// 스크롤 목표 위치 계산
    /// </summary>
    private float CalculateTargetPosition(RectTransform targetItem, RectTransform content)
    {
        // Content의 높이
        float contentHeight = content.rect.height;
        
        // Viewport의 높이
        float viewportHeight = _scrollRect.viewport.rect.height;
        
        // 스크롤 가능한 전체 거리
        float scrollableHeight = contentHeight - viewportHeight;
        
        if (scrollableHeight <= 0) return 0f;
        
        // 타겟 아이템의 위치 (위에서부터의 거리)
        float targetPositionFromTop = -targetItem.anchoredPosition.y;
        
        // 아이템이 화면 중앙에 오도록 조정
        float targetCenter = targetPositionFromTop - (viewportHeight / 2) + (targetItem.rect.height / 2);
        
        // 정규화된 위치 계산 (0 ~ 1)
        // 세로 스크롤은 1이 맨 위, 0이 맨 아래
        float normalizedPosition = 1 - (targetCenter / scrollableHeight);
        
        // 범위 제한
        normalizedPosition = Mathf.Clamp01(normalizedPosition);
        
        return normalizedPosition;
    }
}