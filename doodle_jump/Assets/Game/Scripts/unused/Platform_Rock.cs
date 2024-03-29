using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Rock : MonoBehaviour
{
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (this.transform.position.y < collision.transform.position.y)
            {
                this.GetComponent<BoxCollider2D>().isTrigger = false;
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.position.y < this.transform.position.y)
        {
            this.GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }
}
