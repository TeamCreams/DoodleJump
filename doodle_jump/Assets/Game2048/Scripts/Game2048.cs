using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Game2048 : MonoBehaviour
{
    private List<GameObject> _numberBlocks = new List<GameObject>(); // 블록

    public IReadOnlyList<GameObject> NumberBlocks => _numberBlocks;

    private NumberBlockController _numberBlockController;
    private GameObject NumberBlock = null;
    private void Awake()
    {
        GameObject controllerObject = new GameObject("@NumberBlockController");
        _numberBlockController = controllerObject.AddComponent<NumberBlockController>();

        /*NumberBlock = Resources.Load<GameObject>("Prefabs/NumberBlock");
        GameObject parent = GameObject.Find("Background");*/
        for (int i = 0; i < 16; i++)
        {
            //_numberBlocks.Add(Instantiate(NumberBlock));
            var instance = Managers.Resource.Instantiate("NumberBlock");
            float x = -1.94f + (i / 4) * 1.3f;
            float y = -1.94f + (i % 4) * 1.3f;
            instance.transform.position = new Vector2(x, y);
            _numberBlocks.Add(instance);
            //Debug.Log(parent.transform.position.y);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        _numberBlockController.Init(_numberBlocks); // 실행하면 오류 뜸
    }

    // Update is called once per frame
    void Update()
    {
        _numberBlockController.UpdateFunc();
    }

}
