using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private Vector3 _startPos;
    private Vector3 _endPos;
    private float _time = 2f;
    private float t = 0.0f;
   
    protected virtual void Start()
    {
        this.Init();
    }

    protected virtual void Init()
    {
        this.GetComponent<BoxCollider2D>().isTrigger = true;
    }


    protected virtual void GoDownPlatform()
    {
        _startPos = this.transform.position;
        _endPos = new Vector2(this.transform.position.x, this.transform.position.y - 6);        

        if (t > _time)
        {
            t += 0.5f * Time.deltaTime;
            transform.position = Vector3.Lerp(_startPos, _endPos, t);
        }
    }
}
