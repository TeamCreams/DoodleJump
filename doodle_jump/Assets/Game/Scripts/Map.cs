using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Map : MonoBehaviour
{

    private Queue<GameObject> _generateObjects;
    private float _PrevY = 0.5f;
    public int _platformCount = 0;
    private int _DefplatformCount = 16;

    //[SerializeField]
    private GameObject _platformWood;
    //[SerializeField]
    private GameObject _platformBush;

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
        _platformWood = GameObject.FindWithTag("platform_wood"); 
        _platformBush = GameObject.FindWithTag("platform_bush");
    }

    // Update is called once per frame
    void Update()
    {
        if(_platformCount < _DefplatformCount)
        {
            GeneratePlatform();
        }
    }

    private void GeneratePlatform()
    {   // 같은 라인에 하나 이상 생성되지 않음. -1.5f ~ 1.5
        // 충돌할 때 머리는 충돌하지 않음.
        // 간격은 0.5f
        float _random = Random.Range(0, 1.0f);
        _platformCount++;
        GameObject _GM;
        if (_random <= 0.3f)
        {
            _GM = Instantiate(_platformBush, GeneratePosition(), Quaternion.identity);
            //_generateObjects.Enqueue(Instantiate(_platformBush, GeneratePosition(), Quaternion.identity));
        }
        else
        {
            _GM = Instantiate(_platformWood, GeneratePosition(), Quaternion.identity);
        }
        _generateObjects.Enqueue(_GM);
    }

    private void DestoryPlatform()
    {
        //조건 : 카메라에 보이지 않을 때
        Destroy(_generateObjects.Dequeue());
        _platformCount--;
    }

    Vector3 GeneratePosition()
    {
        Vector3 pos = new Vector3(Random.Range(-1.5f, 1.5f), _PrevY, 0);
        int _Random = Random.Range(1, 3); // 1 ~ 2
        _PrevY += 0.5f * _Random;
        return pos;
    }
}
