using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

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
        if (_collider == null)
        {
            // Error Something... 
        }
    }

    private void OnEnable()
    {
        _renderer.enabled = true;
        _collider.enabled = true;
        _platformBushAnimator.SetTrigger("ResetAnim");
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void SetPool(ObjectPool<PlatformController> pool)
    {
        base._pool = pool;
    }

    public void StartDestoryPlatform()
    {
        StartCoroutine(DestroyPlatform());
    }

    IEnumerator DestroyPlatform()
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
