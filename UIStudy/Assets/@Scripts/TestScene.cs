using System;
using UnityEngine;


class TestScene : MonoBehaviour
{
    UI_Inventory _inven;
    void Awake()
    {
        Managers.UI.ShowSceneUI<UI_SampleScene>();

        _inven = Managers.UI.MakeSubItem<UI_Inventory>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            _inven.gameObject.SetActive(!_inven.gameObject.activeSelf);

        }
    }
}