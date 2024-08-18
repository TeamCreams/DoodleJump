using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_SampleScene : UI_Scene
{
    enum Images
    {
        Image_Test
    }

    enum Buttons
    {
        Button_Inventory,
        Button_Menu
    }

    enum Texts
    {
    }

    enum GameObjects
    {
    }

    protected override void Init()
    {
        base.Init();

        this.BindButtons(typeof(Buttons));
        this.BindImages(typeof(Images));

        //this.Get<Image>((int)Images.Image_Test);
        this.Get<Image>((int)Images.Image_Test).gameObject.BindEvent(
            (evt) =>
            {
                Debug.Log("log 3");

                Managers.UI.ShowPopupUI<UI_Popup>();

                GetImage((int)Images.Image_Test).color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            },
            Define.EUIEvent.PointerDown);   

        this.Get<Button>((int)Buttons.Button_Inventory).gameObject.BindEvent((evt) =>
        {
            Debug.Log("log 1");

            //Managers.UI.MakeSubItem<UI_InventorySlot>();
        }, Define.EUIEvent.Click);


        this.Get<Button>((int)Buttons.Button_Menu).onClick.AddListener(() =>
        {
            Debug.Log("OnClick 2");
        });
    }

}

