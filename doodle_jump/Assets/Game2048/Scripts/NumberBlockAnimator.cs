using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberBlockAnimator : MonoBehaviour
{
    private int _number { get; set; }
    public int Number
    {
        get { return _number; }
        set { _number = value; }
    }

    // MoveCount
    private int _moveCount = 0;
    public int MoveCount
    {
        get { return _moveCount; }
        set { _moveCount = value; }
    }
    public void AddMoveCount() { _moveCount++; }

    private NumberBlockDirState _directionState = NumberBlockDirState.None;

    private Sprite[] _sprites;
    private SpriteRenderer _spriteRenderer;


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
        _spriteRenderer = GetComponent<SpriteRenderer>();
        LoadResources();
        ChangeImage(0);
    }

    private void LoadResources()
    {
        _sprites = Resources.LoadAll<Sprite>("Image2048");
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
            default:
                _spriteRenderer.sprite = null;
                break;
        }
    }

    public void ChangeDirectionState(NumberBlockDirState directionState, Vector2 endPos)
    {

        _directionState = directionState;

        if (_directionState == NumberBlockDirState.Up)
        {
            this.transform.DOMove(endPos, 0.1f).SetEase(Ease.Linear);
            MoveCount = 0;
        }
        else if (_directionState == NumberBlockDirState.Down)
        {
            this.transform.DOMove(endPos, 0.1f).SetEase(Ease.Linear);
            MoveCount = 0;
        }
        else if (_directionState == NumberBlockDirState.Left) //left
        {
            this.transform.DOMove(endPos, 0.1f).SetEase(Ease.Linear);
            MoveCount = 0;
        }
        else if (_directionState == NumberBlockDirState.Right) //right
        {
            this.transform.DOMove(endPos, 0.1f).SetEase(Ease.Linear);
            MoveCount = 0;
        }
    }
}
