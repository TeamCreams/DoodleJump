using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerCtrl : MonoBehaviour
{
    //[SerializeField]
    private float _movePower = 1.2f;
    public float _jumpPower = 5f; // 점프 가속 발판 있는 플랫폼에서 참조할 수 있게 public

    private Rigidbody2D _rigid;

    private Animator _ani;

    private static PlayerCtrl _instance = null;
    public static PlayerCtrl Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType(typeof(PlayerCtrl)) as PlayerCtrl;
                //MonoBehaviour 일땐 new 사용 못함.
                }
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _rigid.freezeRotation = true;
        _ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CtrlMove();
    }

    //platform 상호작용
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("platform_bush") 
            || collision.gameObject.CompareTag("platform_wood") 
            || collision.gameObject.CompareTag("platform_rock"))
        {
            _ani.SetBool("isIdle", true); 
            StartCoroutine(Jump());
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("platform_wood")
            || collision.gameObject.CompareTag("platform_rock"))
        {
            _ani.SetBool("isIdle", true);
            StartCoroutine(Jump());
        }
    }

    // 이동
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
            _ani.SetBool("isRun", false);
            _ani.SetBool("isIdle", true);
        }
        transform.position += moveVelocity * _movePower * Time.deltaTime;
    }

    // 점프
    IEnumerator Jump()
    {
        yield return new WaitForSeconds(0.2f);
        _rigid.velocity = Vector2.zero;

        Vector2 jumpVelocity = new Vector2(0, _jumpPower);
        _rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);
        _ani.SetTrigger("isJump");
        _ani.SetBool("isIdle", false);
    }
}
