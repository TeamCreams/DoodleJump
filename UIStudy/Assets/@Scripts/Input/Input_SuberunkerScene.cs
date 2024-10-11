using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_SuberunkerScene : MonoBehaviour
{
    public void OnKeyAction()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            Managers.Camera.Shake(1.0f, 0.2f);
        }
    }
}
