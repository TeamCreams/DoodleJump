using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_EvolutionItem : UI_Base
{
    private enum GameObjects
    {
        Selected
    }
    private enum Images
    {
        isClick
    }
        private Toggle _toggle = null;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindObjects(typeof(GameObjects));
        BindImages(typeof(Images));

        GetImage((int)Images.isClick).gameObject.BindEvent(OnClick_IsClickItem, EUIEvent.Click);
        GetImage((int)Images.isClick).gameObject.BindEvent(OnPointerUp_IsClickItem, EUIEvent.PointerUp); //OnPointerUp
        _toggle = this.gameObject.GetComponent<Toggle>();
        Managers.Event.AddEvent(EEventType.Evolution, OnEvent_SetToggle);
        return true;
    }
    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.Evolution, OnEvent_SetToggle);
    }
    private void OnEvent_SetToggle(Component sender = null, object param = null)
    {
        _toggle.group = this.transform.parent.parent.gameObject.GetComponent<ToggleGroup>();
    }

    private void OnClick_IsClickItem(PointerEventData eventData)
    {
        GetObject((int)GameObjects.Selected).SetActive(true);
        
    }
    private void OnPointerUp_IsClickItem(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp_IsClickItem");
        GetObject((int)GameObjects.Selected).SetActive(false);
    }
}
