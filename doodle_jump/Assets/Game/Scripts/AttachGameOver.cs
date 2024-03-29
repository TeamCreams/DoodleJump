using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttachGameOver : MonoBehaviour
{
    public bool _playerDrop = false;
    private static AttachGameOver _instance = null;
    public static AttachGameOver Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType(typeof(AttachGameOver)) as AttachGameOver;
                //MonoBehaviour 일땐 new 사용 못함.
            }
            return _instance;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            _playerDrop = true;
            collision.gameObject.SetActive(false);
            //Debug.Log("Game Over");
;        }
    }
}
