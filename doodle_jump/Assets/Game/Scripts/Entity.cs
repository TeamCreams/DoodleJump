using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Managers.Instance.UIManager.SettingUI();
    }

    // Update is called once per frame
    void Update()
    {
        Managers.Instance.UIManager.Score();
        if (AttachGameOver.Instance._playerDrop)
        {
            StartCoroutine(Managers.Instance.UIManager.UpUI());
        }
        
    }
}
