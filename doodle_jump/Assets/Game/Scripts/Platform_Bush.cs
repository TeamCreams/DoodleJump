using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Bush : MonoBehaviour
{
    private Animator _platformAni;

    // Start is called before the first frame update
    void Start()
    {
        _platformAni = gameObject.GetComponent<Animator>();
        this.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (this.transform.position.y < collision.transform.position.y)
            {
                this.GetComponent<BoxCollider2D>().isTrigger = false;
                StartCoroutine(BrokenPlatform(collision.gameObject));
            }
        }
    }

    IEnumerator BrokenPlatform(GameObject gameObject)
    {
        yield return new WaitForSeconds(0.5f);
        this.GetComponent<BoxCollider2D>().enabled = false;
        _platformAni.SetTrigger("isCollision");
    }
}
