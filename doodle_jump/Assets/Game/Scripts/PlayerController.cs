using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _movePower = 6f;
    public float _jumpPower = 5f; // 점프 가속 발판 있는 플랫폼에서 참조할 수 있게 public

    private Rigidbody2D _rigid;

    private Animator _animation;
    public GameObject _platformRock;
    private RaycastHit2D _hitPlatform;

    private void Awake()
    {
        //자기꺼 받아올때
        //자기꺼 세팅할떄.
        _rigid = GetComponent<Rigidbody2D>();
        _rigid.freezeRotation = true;
        _animation = GetComponent<Animator>();
    }

    void Start()
    {
        //다른객체에서 뭔가 가져와야할때.
        //다른객체를 세팅해야할때
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        
        PlatformCollisionCheck();
    }

    //platform 상호작용
    private void PlatformCollisionCheck()
    {
       if (_rigid.velocity.y < 0)
       {
            _animation.SetBool("isFall", true);

            _hitPlatform = Physics2D.Raycast(_rigid.position, new Vector2(0, -1), 0.3f, LayerMask.GetMask("Platform"));

            if (_hitPlatform.collider != null)
            {
                var component = _hitPlatform.collider.GetComponent<Platform_Bush>();
                if (component != null)
                {
                    this.StartJumpTrigger();
                    component.StartDestoryPlatform();
                }
                else if(_hitPlatform.collider != null)
                {
                    this.StartJumpTrigger();
                }
            }
        }
    }

    // 이동
    public void PlayerMovement()
    {
        float _horizontal = Input.GetAxis("Horizontal");
        Vector2 _direction = new Vector2(_horizontal, 0);
        _rigid.AddForce(_direction * _movePower * Time.deltaTime, ForceMode2D.Impulse);
        if (_horizontal < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (0 < _horizontal)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    public void StartJumpTrigger()
    {
        _animation.SetTrigger("isJump");
        _animation.SetBool("isFall", false);

        _rigid.velocity = Vector2.zero;
        _rigid.simulated = false;

        //Debug.Log(nameof(StartJumpTrigger));
    }

    public void AnimEvent_JumpStart()
    {
        _rigid.simulated = true;
        _rigid.velocity = Vector2.zero;
        Vector2 jumpVelocity = new Vector2(0, _jumpPower);
        _rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);
    }
    // 오랫동안 가만히 있으면 모션이나 표정 바꾸기
}
