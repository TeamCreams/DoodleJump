using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(UnityEngine.Random.Range(0, 5) == 0)
        {
            Debug.LogError("error!");
            Util.WhoAmI(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
