using System;
using UnityEngine;


class TestScene : MonoBehaviour
{
    UI_Inventory _inven;
    [SerializeField]
    UnityEngine.UI.Image _image;

    void Awake()
    {
        StartLoadAssets();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            _inven.gameObject.SetActive(!_inven.gameObject.activeSelf);
        }

        if(Input.GetKeyDown(KeyCode.J))
		{
            //Managers.Resource.Load<Sprite>("Sprite_Loading")\
            Sprite sprite1 = Managers.Resource.Load<Sprite>("Sprite_Loading.sprite");
            //Managers.Resource.Instantiate("Sprite_Loading.sprite");
            Debug.Log(sprite1);
            _image.sprite = sprite1;
            Debug.Log(Managers.Data.TestDic[1].Name);
        }
    }


    void StartLoadAssets()
    {
        Managers.Resource.LoadAllAsync<UnityEngine.Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                Debug.Log("Load Complete");
                Managers.Data.Init();

                Managers.UI.ShowSceneUI<UI_SampleScene>();

                _inven = Managers.UI.MakeSubItem<UI_Inventory>();
            }
        });
    }
}