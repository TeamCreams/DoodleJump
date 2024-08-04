using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class NumberBlockActor : MonoBehaviour
{
    [SerializeField]
    private int _number = 0;
   
    private Sprite[] _sprites;
    private SpriteRenderer _spriteRenderer;
    // Number
    public int Number
    {
        get { return _number; }
        set { _number = value; }
    }
    private void Awake()
    {
        // Image Resource 가져오기
        _spriteRenderer = GetComponent<SpriteRenderer>();
        LoadResources();
        ChangeImage(0);
    }

    private void LoadResources()
    {
        _sprites = Resources.LoadAll<Sprite>("2048");
    }

    public void ChangeImage(int sum)
    {
        switch (sum) 
        {
            case 2:
                _spriteRenderer.sprite = _sprites[0];
                break;
            case 4:
                _spriteRenderer.sprite = _sprites[1];
                break;
            case 8:
                _spriteRenderer.sprite = _sprites[2];
                break;
            case 16:
                _spriteRenderer.sprite = _sprites[3];
                break;
            case 32:
                _spriteRenderer.sprite = _sprites[4];
                break;
            case 64:
                _spriteRenderer.sprite = _sprites[5];
                break;
            case 128:
                _spriteRenderer.sprite = _sprites[6];
                break;
            case 256:
                _spriteRenderer.sprite = _sprites[7];
                break;
            case 512:
                _spriteRenderer.sprite = _sprites[8];
                break;
            case 1024:
                _spriteRenderer.sprite = _sprites[9];
                break;
            case 2048:
                _spriteRenderer.sprite = _sprites[10];
                break;
            default :
                _spriteRenderer.sprite = null;
                break;
        }
    }
}
