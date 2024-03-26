using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    private float _movePower = 1f;
    private float _jumpPower = 3f;

    private Rigidbody2D _rigid;

    private Animator _ani;

    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _rigid.freezeRotation = true;
        _ani = GetComponent<Animator>();
        //StartCoroutine(Jump());
    }

    // Update is called once per frame
    void Update()
    {
        CtrlMove();
    }

    IEnumerator Jump()
    {
        _rigid.velocity = Vector2.zero;

        Vector2 jumpVelocity = new Vector2(0, _jumpPower);
        _rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);
        _ani.SetTrigger("isJump");
        _ani.SetBool("isIdle", false);
        yield return new WaitForSeconds(1f);
    }

    //platform 상호작용
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("platform_wood"))
        {
            _ani.SetBool("isIdle", true); // 발판에 닿으면 isIdle.
            StartCoroutine(Jump());
        }
        else if(collision.gameObject.CompareTag("platform_bush"))
        {
            _ani.SetBool("isIdle", true);
            StartCoroutine(Jump());
            StartCoroutine(BrokenPlatform(collision.gameObject));
        }
    }

    IEnumerator BrokenPlatform(GameObject gameObject)
    {
        Animator platAni = gameObject.GetComponent<Animator>();
        yield return new WaitForSeconds(0.7f);
        platAni.SetTrigger("isCollision");
    }

    public void CtrlMove()
    {
        Vector3 moveVelocity = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            moveVelocity = Vector2.left;
            this.transform.rotation = new Quaternion(0, 180, 0, 0);

            _ani.SetBool("isRun", true); 
            _ani.SetBool("isIdle", false);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            moveVelocity = Vector2.right;
            this.transform.rotation = new Quaternion(0, 0, 0, 0);

            _ani.SetBool("isRun", true); 
            _ani.SetBool("isIdle", false);
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            _ani.SetBool("isIdle", true);
            _ani.SetBool("isRun", false);
        }
            transform.position += moveVelocity * _movePower * Time.deltaTime;
    }
}
