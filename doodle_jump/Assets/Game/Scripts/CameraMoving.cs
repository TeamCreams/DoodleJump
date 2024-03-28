using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    private Transform _target;
    private float _smoothSpeed = 0;
    private Vector3 _offset = new Vector3(0, 0, -2); // �÷��̾�� ī�޶� ������ �Ÿ�

    private static CameraMoving _instance = null;
    public static CameraMoving Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType(typeof(CameraMoving)) as CameraMoving;
                //MonoBehaviour �϶� new ��� ����.
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
        _smoothSpeed = PlayerCtrl.Instance._jumpPower * 0.05f; // ĳ���Ͱ� ���� �����̸� ���� ����������
        Vector3 _CameraPosition = _target.position + _offset;
        Vector3 _smoothedPosition = Vector3.Lerp(transform.position, _CameraPosition, _smoothSpeed);
        transform.position = _smoothedPosition;
    }
}
