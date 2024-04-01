using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Bush : PlatformController
{
    private Animator _platformBushAnimation;

    // Start is called before the first frame update
    void Start()
    {
        _platformBushAnimation = gameObject.GetComponent<Animator>();
        this.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        var _isPlayer = collision.GetComponent<PlayerController>();
        if (_isPlayer != null)
        { 
            if (this.transform.position.y < collision.transform.position.y)
            {
                this.GetComponent<BoxCollider2D>().isTrigger = false;
                StartCoroutine(BrokenPlatform());
            }
        }
    }

    IEnumerator BrokenPlatform()
    {
        yield return new WaitForSeconds(0.4f);
        this.GetComponent<BoxCollider2D>().enabled = false;
        _platformBushAnimation.SetTrigger("isCollision");
    }
}
