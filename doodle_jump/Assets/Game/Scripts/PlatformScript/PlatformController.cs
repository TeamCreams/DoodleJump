using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class PlatformController : MonoBehaviour
{
    public ObjectPool<PlatformController> _pool;
    private InGameScene _gameScene;

    public virtual void SetPool(ObjectPool<PlatformController> pool)
    {
        _pool = pool;
    }

    private Vector3 _startPos;
    private Vector3 _endPos;
    private float _time = 2f;
    private float t = 0.0f;
   
    protected virtual void Start()
    {
        this.Init();
        _gameScene = Util.FindChildWithPath<InGameScene>("@InGameScene");
    }

    protected virtual void Init()
    {
        this.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    protected virtual void OnEnable()
    {
        this.StartCoroutine(BecomeInvisible());
    }
    protected virtual IEnumerator BecomeInvisible()
    {
        while (true)
        {
            yield return null;
            if (this.transform.position.y + 2 < _gameScene.PlayerController.transform.position.y)
            {//Release 코루틴이 첫 start 시에만 호출되지 않도록 주의.
                if (_pool != null)
                {
                    _pool.Release(this);
                }
            }
        }
    }

    protected virtual void GoDownPlatform()//아래로 내려가는 Platform
    {
        _startPos = this.transform.position;
        _endPos = new Vector2(this.transform.position.x, this.transform.position.y - 6);        

        if (t > _time)
        {
            t += 0.5f * Time.deltaTime;
            transform.position = Vector3.Lerp(_startPos, _endPos, t);
        }
    }
}
