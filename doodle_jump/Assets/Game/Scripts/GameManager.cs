using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    void Start()
    {
        //var mapGameObject = GameObject.Find("@Map").transform.Find("BBB").gameObject;

        //Util.FindChild(mapGameObject, "BBB", true);

        Util.FindChildWithPath("@Map/AAA/BBB");



        
    }

}
