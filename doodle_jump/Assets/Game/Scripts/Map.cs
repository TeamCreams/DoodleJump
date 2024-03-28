using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Map : MonoBehaviour
{

    private Queue<GameObject> _generateObjects = new Queue<GameObject>();
    private float _PrevY = 0.5f;
    public int _platformCount = 0;
    private int _DefplatformCount = 16;
    private int _DefplatformDistance = 3;

    private GameObject _platformWood;
    private GameObject _platformBush;
    private GameObject _platformRock;

    private static Map _instance = null;
    public static Map Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType(typeof(Map)) as Map;
                //MonoBehaviour �϶� new ��� ����.
            }
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        _platformWood = Resources.Load<GameObject>("Prefabs/platform_wood");
        _platformBush = Resources.Load<GameObject>("Prefabs/platform_bush");
        _platformRock = Resources.Load<GameObject>("Prefabs/platform_rock");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_platformCount < _DefplatformCount)
        {
            GeneratePlatform();
        }
        DestoryPlatform();
    }

    private void GeneratePlatform()
    {   // ���� ���ο� �ϳ� �̻� �������� ����. -1.5f ~ 1.5
        // �浹�� �� �Ӹ��� �浹���� ����.
        // ������ 0.5f
        float _random = Random.Range(0, 1.0f);
        _platformCount++;
        if (_random <= 0.3f)
        {
            _generateObjects.Enqueue(Instantiate(_platformBush, GeneratePosition(), Quaternion.identity));
        }
        else if(_random <= 0.6f)
        {
            _generateObjects.Enqueue(Instantiate(_platformWood, GeneratePosition(), Quaternion.identity));
        }
        else
        {
            _generateObjects.Enqueue(Instantiate(_platformRock, GeneratePosition(), Quaternion.identity));
        }
    }

    private void DestoryPlatform()
    {   //���� : ī�޶� ������ ���� ��
        GameObject _destoryObject = _generateObjects.Peek();
        //Debug.Log("_destoryObject" + _destoryObject.transform.position.y);
        //Debug.Log("CameraMoving " + CameraMoving.Instance.transform.position.y);
        if (_destoryObject.transform.position.y + _DefplatformDistance < CameraMoving.Instance.transform.position.y)
        {
            Destroy(_destoryObject);
            _destoryObject = null;
            Resources.UnloadUnusedAssets();
            _platformCount--;
            _generateObjects.Dequeue();
        }
    }

    Vector3 GeneratePosition()
    {
        float _Random = Random.Range(1, 1.6f); // 1 ~ 1.6f
        _PrevY += 0.3f * _Random;
        Vector3 pos = new Vector3(Random.Range(-1.5f, 1.5f), _PrevY, 0);
        //���� ���ǰ� ���� ���� �̻� ���� �ʵ��� ���� �ʿ�
        return pos;
    }
}
