using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    //private InGameScene _gameScene;
    // Start is called before the first frame update
    void Awake()
    {
        this.GetComponent<BoxCollider2D>().isTrigger = true;
        //_gameScene = Util.FindChildWithPath<InGameScene>("@InGameScene");
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        var _isPlayer = collision.GetComponent<PlayerController>();
        if (_isPlayer != null)
        {
            if (this.transform.position.y < collision.transform.position.y)
            {
                this.GetComponent<BoxCollider2D>().isTrigger = false;
            }
        }
    }
}
