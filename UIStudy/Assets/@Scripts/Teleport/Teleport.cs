using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Teleport : ObjectBase
{
    private ParticleSystem _teleportEffect;

    enum DirectionPosition
    {
        Left,
        Right
    }
    private DirectionPosition _directionPosition = DirectionPosition.Left;
    private bool _isStoneShower = false;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        CheckDirection();
        Managers.Event.AddEvent(EEventType.IsStoneShower, OnEvent_StoneShower);
        _teleportEffect = Managers.Resource.Instantiate("TeleportEffect").GetComponent<ParticleSystem>();
        _teleportEffect.gameObject.SetActive(false);

        OnTriggerEnter_Event -= OnTriggerEnterPlayer;
        OnTriggerEnter_Event += OnTriggerEnterPlayer;

        return true;
    }

    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.IsStoneShower, OnEvent_StoneShower);
    }

    private void CheckDirection()
    {
        if (this.transform.position.x < 0)
        {
            _directionPosition = DirectionPosition.Left;
        }
        else
        {
            _directionPosition = DirectionPosition.Right;
        }
    }

    private void OnTriggerEnterPlayer(Collider collision)
    {
        if(_isStoneShower == true)
        {
            return;
        }
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

            if (playerController.transform.position.x < HardCoding.PlayerTeleportPos_Left.x)
            {
                TeleportLeft(playerController);
            }
            else if (HardCoding.PlayerTeleportPos_Right.x < playerController.transform.position.x)
            {
                TeleportRight(playerController);
            }
            else
            {
                switch (_directionPosition)
                {
                    case DirectionPosition.Left:
                        TeleportLeft(playerController);
                        break;

                    case DirectionPosition.Right:
                        TeleportRight(playerController);
                        break;
                }
            }
        }
    }

    private void TeleportRight(PlayerController playerController)
    {
        playerController.Teleport(HardCoding.PlayerTeleportPos_Left);
        StartCoroutine(TeleportEffect(HardCoding.PlayerTeleportPos_Left));
    }

    private void TeleportLeft(PlayerController playerController)
    {
        playerController.Teleport(HardCoding.PlayerTeleportPos_Right);
        StartCoroutine(TeleportEffect(HardCoding.PlayerTeleportPos_Right));
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(HardCoding.PlayerTeleportPos_Left, 5.5f);
        Gizmos.DrawSphere(HardCoding.PlayerTeleportPos_Right, 5.5f);
    }

    IEnumerator TeleportEffect(Vector2 pos)
    {
        _teleportEffect.gameObject.SetActive(true);
        _teleportEffect.gameObject.transform.position = pos;
        _teleportEffect.Play();
        yield return new WaitForSeconds(_teleportEffect.main.duration);
        _teleportEffect.gameObject.SetActive(false);

    }

    private void OnEvent_StoneShower(Component sender, object param)
    {
        _isStoneShower = !_isStoneShower;
    }
}
