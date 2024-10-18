using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class CameraManager
{

    //IEnumerator _shakeCo = null;

    public void Shake(float shakePower, float shakeDuration)
    {
        GameObject slave = new GameObject("Slave");

        ShakeSlave shakeSlave = slave.GetOrAddComponent<ShakeSlave>();

        shakeSlave.Shake(shakePower, shakeDuration);

    }
}
