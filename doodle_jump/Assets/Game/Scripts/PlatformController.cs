using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    //private InGameScene _gameScene;
    // Start is called before the first frame update
    void Awake()
    {
        //this.GetComponent<BoxCollider2D>().isTrigger = true;
        //_gameScene = Util.FindChildWithPath<InGameScene>("@InGameScene");
    }

    protected virtual void Start()
    {
        this.Init();
    }

    protected virtual void Init()
    {
        this.GetComponent<BoxCollider2D>().isTrigger = true;
    }
}
