using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachGameOver : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("Game Over");
;        }
    }
}
