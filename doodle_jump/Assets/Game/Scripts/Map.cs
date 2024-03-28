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
    private int _DefplatformDistance = 5;

    //[SerializeField]
    private GameObject _platformWood;
    //[SerializeField]
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
                //MonoBehaviour 일땐 new 사용 못함.
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
    {   // 같은 라인에 하나 이상 생성되지 않음. -1.5f ~ 1.5
        // 충돌할 때 머리는 충돌하지 않음.
        // 간격은 0.5f
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
    {   //조건 : 카메라에 보이지 않을 때
        GameObject _destoryObject = _generateObjects.Peek();
        //Debug.Log("_destoryObject" + _destoryObject.transform.position.y);
        //Debug.Log("CameraMoving " + CameraMoving.Instance.transform.position.y);
        if (_destoryObject.transform.position.y + _DefplatformDistance < CameraMoving.Instance.transform.position.y)
        {
            //_destoryObject = null; // error
            Destroy(_destoryObject);
            Resources.UnloadUnusedAssets();
            _platformCount--;
            _generateObjects.Dequeue();
        }
    }

    Vector3 GeneratePosition()
    {
        Vector3 pos = new Vector3(Random.Range(-1.5f, 1.5f), _PrevY, 0);
        float _Random = Random.Range(1, 1.6f); // 1 ~ 1.6f
        _PrevY += 0.5f * _Random;
        return pos;
    }
}
