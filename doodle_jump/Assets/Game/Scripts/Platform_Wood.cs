using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Platform_Wood : MonoBehaviour 
{ 
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<BoxCollider2D>().isTrigger = true;
        /*
         if(UnityEngine.Random.Range(0, 5) == 0)
        {
            Debug.LogError("error!");
            Util.WhoAmI(gameObject);
        }
         */
    }

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
