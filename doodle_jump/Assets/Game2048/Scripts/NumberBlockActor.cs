using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberBlockActor : MonoBehaviour
{
    private int _number = 0;
    private NumberBlockDirState _directionState = NumberBlockDirState.None;
   
    private Vector2 _startPos = Vector2.zero;
    private float _sumTime = 0;
    private float _speed = 10;
    private int _moveCount = 0;

    private Sprite[] _sprites;
    private Image _image;
    // Number
    public int GetNumber() { return _number; }
    public void SetNumber(int number) { _number = number; }

    // MoveCount
    public int GetMoveCount() { return _moveCount; }
    public void AddMoveCount() { _moveCount++; }
    public void SetMoveCount(int moveCount) { _moveCount = moveCount; }

    public enum NumberBlockDirState
    {
        Down,
	    Up,
	    Left,
	    Right,

	    None
    };

    private void Awake()
    {
        // Image Resource 가져오기

        _image = GetComponent<Image>();
        LoadResources();
        Debug.Log(_sprites.Length);
        ChangeImage(2);
    }

    private void LoadResources()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Assets/Game2048/Resources/Image2048.bmp");
        for (int i = 0; i < sprites.Length; i++)
        {
            _sprites[i] = sprites[i];
        }
    }
    public void ChangeImage(int sum)
    {
        // Sum값이 맞춰 Image 변경
        switch (sum) 
        {
            case 2:
                _image.sprite = _sprites[0];
                break;
            case 4:
                _image.sprite = _sprites[1];
                break;
            case 8:
                _image.sprite = _sprites[2];
                break;
            case 16:
                _image.sprite = _sprites[3];
                break;
            case 32:
                _image.sprite = _sprites[4];
                break;
            case 64:
                _image.sprite = _sprites[5];
                break;
            case 128:
                _image.sprite = _sprites[6];
                break;
            case 256:
                _image.sprite = _sprites[7];
                break;
            case 512:
                _image.sprite = _sprites[8];
                break;
            case 1024:
                _image.sprite = _sprites[9];
                break;
            case 2048:
                _image.sprite = _sprites[10];
                break;
            default :
                break;
        }
    }

    public void ChangeDirectionState(NumberBlockDirState directionState)
    {
        _startPos = this.transform.position;
        _sumTime = 0.0f;
        _directionState = directionState;
    }

    public void SlideActor()
    {
        if (_directionState == NumberBlockDirState.Down) //down
        {
            Vector2 endPos = new Vector2(_startPos.x, _startPos.y + 100 * GetMoveCount());
            _sumTime += Time.deltaTime * _speed;
            float clampSumTime = Mathf.Clamp(_sumTime, 0.0f, 1.0f);
            this.transform.DOLocalMoveX(endPos.y, clampSumTime).SetEase(Ease.Linear);
            /*float clampSumTime = Mathf.Clamp(_sumTime, 0.0f, 1.0f); 
            Vector2 newBlockPos = Mathf.Lerp(_startPos, endPos, clampSumTime);
            newBlockPos.x = Mathf.Clamp(newBlockPos.x, -200, 100);
            newBlockPos.y = Mathf.Clamp(newBlockPos.y, -200, 100);
            this.SetPos(newBlockPos);*/
        }
        else if (_directionState == NumberBlockDirState.Up) //up
        {
            Vector2 endPos = new Vector2(_startPos.x, _startPos.y - 100 * GetMoveCount());
            _sumTime += Time.deltaTime * _speed;
            float clampSumTime = Mathf.Clamp(_sumTime, 0.0f, 1.0f);
            this.transform.DOLocalMoveX(endPos.y, clampSumTime).SetEase(Ease.Linear);
        }
        else if (_directionState == NumberBlockDirState.Left) //left
        {
            Vector2 endPos = new Vector2(_startPos.x - 100 * GetMoveCount(), _startPos.y);
            _sumTime += Time.deltaTime * _speed;
            float clampSumTime = Mathf.Clamp(_sumTime, 0.0f, 1.0f);
            this.transform.DOLocalMoveX(endPos.x, clampSumTime).SetEase(Ease.Linear);
        }
        else if (_directionState == NumberBlockDirState.Right) //right
        {
            Vector2 endPos = new Vector2(_startPos.x + 100 * GetMoveCount(), _startPos.y);
            _sumTime += Time.deltaTime * _speed;
            float clampSumTime = Mathf.Clamp(_sumTime, 0.0f, 1.0f);
            this.transform.DOLocalMoveX(endPos.x, clampSumTime).SetEase(Ease.Linear);
        }
    }
}
