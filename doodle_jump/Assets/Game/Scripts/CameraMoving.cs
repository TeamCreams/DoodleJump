using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    private Transform _target;
    private float _smoothSpeed = 0;
    private Vector3 _offset = new Vector3(0, 0, -2); // 플레이어와 카메라 사이의 거리

    private static CameraMoving _instance = null;
    public static CameraMoving Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType(typeof(CameraMoving)) as CameraMoving;
                //MonoBehaviour 일땐 new 사용 못함.
            }
            return _instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Moving();
    }

    void Moving()
    {
        _smoothSpeed = PlayerCtrl.Instance._jumpPower * 0.05f; // 캐릭터가 빨리 움직이면 같이 빨라지도록
        Vector3 _CameraPosition = _target.position + _offset;
        Vector3 _smoothedPosition = Vector3.Lerp(transform.position, _CameraPosition, _smoothSpeed);
        transform.position = _smoothedPosition;
    }
}
