using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class CameraManager
{

    public void Shake(float shakePower, float shakeDuration)
    {
        GameObject slave = new GameObject("Slave");

        ShakeSlave shakeSlave = slave.GetOrAddComponent<ShakeSlave>();

        shakeSlave.Shake(shakePower, shakeDuration);

    }
}
