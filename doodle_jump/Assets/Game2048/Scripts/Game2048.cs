using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Game2048 : MonoBehaviour
{
    private List<NumberBlockActor> _numberBlocks = new List<NumberBlockActor>(); // ºí·Ï
    private NumberBlockController _numberBlockController;

    private void Awake()
    {
        GameObject parent = GameObject.Find("Background");
        for (int i = 0; i < 16; i++)
        {
            GameObject newGameObject = new GameObject("NumberBlock");
          
            if(gameObject != null ) 
            {
                newGameObject.transform.SetParent(parent.transform);
                Image image = newGameObject.AddComponent<Image>();
                RectTransform rectTransform = newGameObject.GetOrAddComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(41.75f, 41.75f);
                rectTransform.position = new Vector2(20.875f + ((i / 4) * 41.75f),
                    -62.625f + ((i % 4) * 41.75f) + parent.transform.position.y);
                newGameObject.GetOrAddComponent<NumberBlockActor>();
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //_numberBlockController.Init(ref _numberBlocks);
    }

    // Update is called once per frame
    void Update()
    {
        //_numberBlockController.Update();
    }

}
