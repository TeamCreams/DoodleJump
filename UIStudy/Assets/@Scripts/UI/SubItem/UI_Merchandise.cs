using Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_Merchandise : UI_Base
{
    private enum Texts
    {
        Gold_Text,
        Price_Text,
    }
    private enum Images
    {
        Gold_Image
    }
    private ScrollRect _parentScrollRect = null;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));

        this.gameObject.BindEvent(GoToGooglePrice, EUIEvent.Click);
        this.gameObject.BindEvent(OnBeginDrag, EUIEvent.BeginDrag);
        this.gameObject.BindEvent(OnDrag, EUIEvent.Drag);
        this.gameObject.BindEvent(OnEndDrag, EUIEvent.EndDrag);
        
        return true;
    }

    public void SetInfo(int id)
    {
        CashItemData data = Managers.Data.CashItemDataDic[id];
        GetImage((int)Images.Gold_Image).sprite =  Managers.Resource.Load<Sprite>($"{data.GoldSprite}.sprite");
        GetText((int)Texts.Gold_Text).text = data.Gold.ToString();
        GetText((int)Texts.Price_Text).text = data.Price.ToString();
        _parentScrollRect = this.transform.GetComponentInParent<ScrollRect>();
    }

    private void GoToGooglePrice(PointerEventData eventData)
    {
        //Data에 맞는 값 호출 
        Managers.IAP.Buy("CASH_1000");
    }

    private void OnBeginDrag(PointerEventData eventData)
	{
        _parentScrollRect.OnBeginDrag(eventData); // 부모한테 이벤트 전달
    }
    private void OnDrag(PointerEventData eventData)
    {
        _parentScrollRect.OnDrag(eventData);
    }
    private void OnEndDrag(PointerEventData eventData)
    {
        _parentScrollRect.OnEndDrag(eventData);
    }
}
