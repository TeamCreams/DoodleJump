using System.Collections;
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

        _toggle = this.gameObject.GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnClick_IsClickItem);
        return true;
    }

    private void OnClick_IsClickItem(bool isOn)
    {
        GetObject((int)GameObjects.Selected).SetActive(isOn);
    }
}
