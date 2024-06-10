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

        NumberBlock = Resources.Load<GameObject>("Prefabs/NumberBlock");
        GameObject parent = GameObject.Find("Background");
        for (int i = 0; i < 16; i++)
        {
            //_numberBlocks.Add(Instantiate(NumberBlock));
            _numberBlocks.Add(Managers.Resource.Instantiate("NumberBlock", parent.transform));
            _numberBlocks[i].transform.SetParent(parent.transform);
            RectTransform rectTransform = _numberBlocks[i].GetOrAddComponent<RectTransform>();
            rectTransform.position = new Vector2(33f + ((i / 4) * 33f),
                -50.5f + ((i % 4) * 33f) + parent.transform.position.y);
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
