using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTestController : MonoBehaviour
{
    [SerializeField]
    List<GameObject> gameObjects = new List<GameObject>();


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log(Managers.Resource);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            gameObjects.Add(Managers.Resource.Instantiate("Dummy"));
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (0 < gameObjects.Count)
            {
                Managers.Resource.Destroy(gameObjects[0]);
                gameObjects.RemoveAt(0);
            }
        }
    }

}
