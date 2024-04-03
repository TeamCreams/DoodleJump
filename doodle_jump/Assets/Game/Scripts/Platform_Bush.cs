using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Bush : PlatformController
{
    private Animator _platformBushAnimator;
    private BoxCollider2D _collider;
    private SpriteRenderer _renderer;
    private void Awake()
    {
        _collider = this.GetComponent<BoxCollider2D>();
        _renderer = this.GetComponent<SpriteRenderer>();
        _platformBushAnimator = gameObject.GetComponent<Animator>();

        if(_collider == null)
        {
            // Error Something... 
        }
    }

    protected override void Start()
    {
        base.Start();

    }


    //protected override void OnTriggerEnter2D(Collider2D collision)
    //{
    //    var _isPlayer = collision.GetComponent<PlayerController>();
    //    if (_isPlayer != null)
    //    { 
    //        if (this.transform.position.y < collision.transform.position.y)
    //        {
    //            this.GetComponent<BoxCollider2D>().isTrigger = false;
    //            StartCoroutine(BrokenPlatform());
    //        }
    //    }
    //}

    public void StartBroken()
    {
        StartCoroutine(BrokenPlatform());
    }
    IEnumerator BrokenPlatform()
    {
        yield return new WaitForSeconds(0.4f);
        _collider.enabled = false;
        _platformBushAnimator.SetTrigger("isCollision");
    }

    public void AnimEvent_BrokenBush()
    {
        _renderer.enabled = false;
    }
}
