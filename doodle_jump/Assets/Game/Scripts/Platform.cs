using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private int _direction = 1;
    private Animator _platformAni;

    public enum PlatformState
    {
        platform_rock,
        platform_wood,
        platform_bush,

        None
    }
    [SerializeField]
    private PlatformState _platformState;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<BoxCollider2D>().isTrigger = true;

        switch (_platformState)
        {
            case PlatformState.platform_rock:

                break;

            case PlatformState.platform_wood:
                int _random = Random.Range(0, 2);
                switch (_random)
                {
                    case 0:
                        _direction = 1;
                        break;
                    case 1:
                        _direction = -1;
                        break;
                }
                StartCoroutine(PlatformMoving());
                break;

            case PlatformState.platform_bush:
                _platformAni = gameObject.GetComponent<Animator>();
                break;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (this.transform.position.y < collision.transform.position.y)
            {
                this.GetComponent<BoxCollider2D>().isTrigger = false;
                if(_platformState == PlatformState.platform_bush)
                {
                    StartCoroutine(BrokenPlatform(collision.gameObject));
                }
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(_platformState == PlatformState.platform_wood || _platformState == PlatformState.platform_rock)
        {
            if (collision.transform.position.y < this.transform.position.y)
            {
                this.GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }
    }

    IEnumerator PlatformMoving()
    {
        while(true)
        {
            Vector3 _startPos = transform.position;
            Vector3 _endPos = _startPos + new Vector3(_direction, 0, 0);
            float _time = 0;

            while (_time < 0.5f)
            {
                _time += Time.deltaTime;
                float t = Mathf.Clamp01(_time / 0.8f);
                transform.position = Vector3.Lerp(_startPos, _endPos, t);
                yield return null;
            }
            _direction *= -1;
        }
    }

    IEnumerator BrokenPlatform(GameObject gameObject)
    {
        yield return new WaitForSeconds(0.3f);
        this.GetComponent<BoxCollider2D>().enabled = false;
        _platformAni.SetTrigger("isCollision");
    }
}
