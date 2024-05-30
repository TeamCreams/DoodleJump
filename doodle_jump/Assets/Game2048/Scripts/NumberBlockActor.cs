using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberBlockActor : MonoBehaviour
{
    private int _number = 0;
    private NumberBlockDirState _directionState = NumberBlockDirState.None;
   
    private Vector2 _startPos = Vector2.zero;
    private float _sumTime = 0;
    private float _speed = 10;
    private int _moveCount = 0;

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
    }

    public void ChangeImage(int sum)
    {
        // Sum값이 맞춰 Image 변경
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
