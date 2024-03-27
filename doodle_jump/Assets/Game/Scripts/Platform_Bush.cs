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
        this.GetComponent<BoxCollider2D>().isTrigger = true;

        yield return new WaitForSeconds(0.7f);
        Map.Instance._platformCount--;
        _platformAni.SetTrigger("isCollision");
    }
}
