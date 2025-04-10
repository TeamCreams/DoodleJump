using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class NumberBlockController : MonoBehaviour
{
    //ref 공부
    
    // #define
    static float RELEASE_TIME  = 0.4f;
    static int MAX_BLOCK_COUNT = 16;

    public enum PressKey
    {
        Down,
	    Up,
	    Left,
	    Right,
	    None
    };

    public enum Game2048State
    {
        Release,
        Animation,
        None
    };

    private List<NumberBlockActor> _numberBlocks = new List<NumberBlockActor>(); // 블록
    private List<NumberBlockAnimator> _tempNumberBlocks = new List<NumberBlockAnimator>(); // 애니메이션 이동용 블럭

    //tempBlcok GameObject
    List<GameObject> _tempNumberBlocksGameObject = new List<GameObject>();

    private int[,] _blocksInfo = new int[4, 4]; // 블록 좌표 및 값

	// 새로운 블럭 생성&생성해도 되는지 체크
	private bool _checkCreateNumberBlock = false;

    // 게임 상태
    private Game2048State _gameState = Game2048State.Release;

    // 움직였는지 판단
    private bool _isMove = false;

	private float _time = 0.0f; // 게임 상태가 변경되는 시간
    private int _gameScore = 0; // 게임 점수

    public int GameScore
    {
        get { return _gameScore; }
        set { _gameScore = value; ; } 
    }

    public void Init(List<GameObject> numberBlocksGameObject)
    {
        for(int i = 0; i < MAX_BLOCK_COUNT; i++)
        {
            _numberBlocks.Add(numberBlocksGameObject[i].GetOrAddComponent<NumberBlockActor>());
        }
        Debug.Log(_numberBlocks.Count);
        //위치 초기화
        NumberBlockToZero();
        InitIsFull();

        GameObject parent = GameObject.Find("Background");
       
        for (int i = 0; i < MAX_BLOCK_COUNT; i++)
        {
            var instance = Managers.Resource.Instantiate("NumberBlock");
            _tempNumberBlocksGameObject.Add(instance);
            instance.transform.position = numberBlocksGameObject[i].transform.position;
            instance.transform.rotation = Quaternion.identity;
        }
        for(int i = 0; i < MAX_BLOCK_COUNT; i++)
        {
            _tempNumberBlocks.Add(_tempNumberBlocksGameObject[i].GetOrAddComponent<NumberBlockAnimator>());
        }
    }
    public void UpdateFunc()
    {
        if (_gameState == Game2048State.Release)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                this.SetPressKeyState(PressKey.Down);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                this.SetPressKeyState(PressKey.Up);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                this.SetPressKeyState(PressKey.Left);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                this.SetPressKeyState(PressKey.Right);
            }
        }
        else if (_gameState == Game2048State.Animation)
        {
            // 몇초뒤 다시 GS_RELEAS로 변경
            _time -= Time.deltaTime;
            if (_time <= 0)
            {
                this.SumNumberBlocks();
                this.CheckCreateNumberBlock();
                this.CreateNumberBlock();
                this.SetGame2048State(Game2048State.Release);
            }
        }
    }

    private void InitIsFull()
    {
        int count = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                _blocksInfo[i, j] = _numberBlocks[count++].Number;
            }
        }
    }

    private void NumberBlockToZero()
    {
        for (int i = 0; i < 2; i++)
        {
            int random = Random.Range(0, 15);
            while (_numberBlocks[random].Number != 0)//빈자리 찾기
            {
                random = Random.Range(0, 15);
            }
            _numberBlocks[random].Number = 2;

            _numberBlocks[random].ChangeImage(_numberBlocks[random].Number);
        }
    }
    private void SetGame2048State(Game2048State gameState)
    {
        _gameState = gameState;

        switch (gameState)
        {
            case Game2048State.Release:
                {
                    foreach (NumberBlockAnimator _tempNumberBlock in _tempNumberBlocks)
                    {
                        _tempNumberBlock.MoveCount = 0;
                        _tempNumberBlock.ChangeImage(0);
                    }
                }
                break;
            case Game2048State.Animation:
                {
                    _time = RELEASE_TIME;
                    //잠시 원본 블럭을 지움
                    foreach (NumberBlockActor _numberBlock in _numberBlocks)
                    {
                        _numberBlock.ChangeImage(0);
                    }

                    //임시 애니메이션 블럭 생성 및 이동
                    for (int i = 0; i < MAX_BLOCK_COUNT; i++)
                    {
                        {
                            Vector3 pos = Managers.Game2048.NumberBlocks[i].transform.position;
                            _tempNumberBlocksGameObject[i].transform.position = new Vector2(pos.x, pos.y);
                            _tempNumberBlocks[i].ChangeImage(_numberBlocks[i].Number);
                        }
                    }
                }
                break;
            case Game2048State.None:
                break;
            default:
                break;
        }
    }

    private void SetPressKeyState(PressKey state)
    {
        this.SetGame2048State(Game2048State.Animation);
        for (int i = 0; i < _tempNumberBlocks.Count; i++)
        {
            _tempNumberBlocks[i].MoveCount = 0;
        }
        switch (state)
        {
            case PressKey.Down:
                {
                     
                    this.MoveDown();
                    for (int i = 0; i< _tempNumberBlocks.Count; i++)
                    {
                        //Debug.Log($"FROM : {i}, TO : {i - _tempNumberBlocks[i].MoveCount * 4}");
                        Vector3 endPos = Managers.Game2048.NumberBlocks[i - _tempNumberBlocks[i].MoveCount].transform.position;

                        _tempNumberBlocks[i].ChangeDirectionState(NumberBlockAnimator.NumberBlockDirState.Down, endPos);
                    }
                }
                break;
            case PressKey.Up:
                {
                    this.MoveUp();
                    for (int i = 0; i < _tempNumberBlocks.Count; i++)
                    {
                        Vector3 endPos = Managers.Game2048.NumberBlocks[i + _tempNumberBlocks[i].MoveCount].transform.position;

                        _tempNumberBlocks[i].ChangeDirectionState(NumberBlockAnimator.NumberBlockDirState.Up, endPos);
                    }
                }
                break;
            case PressKey.Left:
                {
                    this.MoveLeft();
                    for (int i = 0; i < _tempNumberBlocks.Count; i++)
                    {
                        Vector3 endPos = Managers.Game2048.NumberBlocks[i - _tempNumberBlocks[i].MoveCount * 4].transform.position;

                        _tempNumberBlocks[i].ChangeDirectionState(NumberBlockAnimator.NumberBlockDirState.Left, endPos);
                    }
                }
                break;
            case PressKey.Right:
                {
                    this.MoveRight();
                    for (int i = 0; i < _tempNumberBlocks.Count; i++)
                    {
                        Vector3 endPos = Managers.Game2048.NumberBlocks[i + _tempNumberBlocks[i].MoveCount * 4].transform.position;

                        _tempNumberBlocks[i].ChangeDirectionState(NumberBlockAnimator.NumberBlockDirState.Right, endPos);
                    }
                }
                break;
            default:
                break;
        }
        this.SubtractTenThousand();

    }

    private void MoveRight()
    {
        for (int i = 3; 0 < i; i--)
        {
            for (int j = 0; j < 4; j++)
            {
                if (_blocksInfo[i, j] == 0)
                {
                    if (_blocksInfo[i - 1, j] != 0)
                    {
                        for (int k = i; k < 4; k++)
                        {
                            if (_blocksInfo[k, j] == 0)
                            {
                                _blocksInfo[k, j] = _blocksInfo[k - 1, j];
                                _blocksInfo[k - 1, j] = 0;
                                _isMove = true;
                                _tempNumberBlocks[(i * 4 + j % 4) - 4].AddMoveCount();
                            }
                            else if (_blocksInfo[k, j] == _blocksInfo[k - 1, j])
                            {
                                _blocksInfo[k, j] += _blocksInfo[k - 1, j];
                                _gameScore += _blocksInfo[k, j]; // 점수
                                _blocksInfo[k, j] += 20000;
                                _blocksInfo[k - 1, j] = 0;
                                _isMove = true; // 이동했는지
                                _tempNumberBlocks[(i * 4 + j % 4) - 4].AddMoveCount();
                            }
                        }
                    }
                }
                else if (_blocksInfo[i, j] == _blocksInfo[i - 1, j])
                {
                    _blocksInfo[i, j] += _blocksInfo[i - 1, j];
                    _gameScore += _blocksInfo[i, j];
                    _blocksInfo[i, j] += 10000;
                    _blocksInfo[i - 1, j] = 0;
                    _isMove = true;
                    _tempNumberBlocks[(i * 4 + j % 4) - 4].AddMoveCount();
                }
            }
        }
    }
    private void MoveLeft()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (_blocksInfo[i, j] == 0)
                {
                    if (_blocksInfo[i + 1, j] != 0)
                    {
                        for (int k = i; 0 <= k; k--)
                        {
                            if (_blocksInfo[k, j] == 0)
                            {
                                _blocksInfo[k, j] = _blocksInfo[k + 1, j];
                                _blocksInfo[k + 1, j] = 0;
                                _isMove = true;
                                _tempNumberBlocks[(i * 4 + j % 4) + 4].AddMoveCount();
                            }
                            else if (_blocksInfo[k, j] == _blocksInfo[k + 1, j])
                            {
                                _blocksInfo[k, j] += _blocksInfo[k + 1, j];
                                _gameScore += _blocksInfo[k, j];
                                _blocksInfo[k, j] += 20000;
                                _blocksInfo[k + 1, j] = 0;
                                _isMove = true;
                                _tempNumberBlocks[(i * 4 + j % 4) + 4].AddMoveCount();
                            }
                        }
                    }
                }
                else if (_blocksInfo[i, j] == _blocksInfo[i + 1, j])
                {
                    _blocksInfo[i, j] += _blocksInfo[i + 1, j];
                    _gameScore += _blocksInfo[i, j];
                    _blocksInfo[i, j] += 10000;
                    _blocksInfo[i + 1, j] = 0;
                    _isMove = true;
                    _tempNumberBlocks[(i * 4 + j % 4) + 4].AddMoveCount();
                }
            }
        }
    }
    private void MoveDown()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (_blocksInfo[i, j] == 0)
                {
                    if (_blocksInfo[i, j + 1] != 0)
                    {
                        for (int k = j; 0 <= k; k--)
                        {
                            if (_blocksInfo[i, k] == 0)
                            {
                                _blocksInfo[i, k] = _blocksInfo[i, k + 1];
                                _blocksInfo[i, k + 1] = 0;
                                _isMove = true;
                                _tempNumberBlocks[(i * 4 + j % 4) + 1].AddMoveCount();
                            }
                            else if (_blocksInfo[i, k] == _blocksInfo[i, k + 1])
                            {
                                _blocksInfo[i, k] += _blocksInfo[i, k + 1];
                                _gameScore += _blocksInfo[i, k];
                                _blocksInfo[i, k] += 20000;
                                _blocksInfo[i, k + 1] = 0;
                                _isMove = true;
                                _tempNumberBlocks[(i * 4 + j % 4) + 1].AddMoveCount();
                            }
                        }
                    }
                }
                else if (_blocksInfo[i, j] == _blocksInfo[i, j + 1])
                {
                    _blocksInfo[i, j] += _blocksInfo[i, j + 1];
                    _gameScore += _blocksInfo[i, j];
                    _blocksInfo[i, j] += 10000;
                    _blocksInfo[i, j + 1] = 0;
                    _isMove = true;
                    _tempNumberBlocks[(i * 4 + j % 4) + 1].AddMoveCount();
                }
            }
        }
    }
    private void MoveUp()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 3; 0 < j; j--)
            {
                if (_blocksInfo[i, j] == 0)
                {
                    if (_blocksInfo[i, j - 1] != 0)
                    {
                        for (int k = j; k < 4; k++)
                        {
                            if (_blocksInfo[i, k] == 0)
                            {
                                _blocksInfo[i, k] = _blocksInfo[i, k - 1];
                                _blocksInfo[i, k - 1] = 0;
                                _isMove = true;
                                _tempNumberBlocks[(i * 4 + j % 4) - 1].AddMoveCount();
                            }
                            else if (_blocksInfo[i, k] == _blocksInfo[i, k - 1])
                            {
                                _blocksInfo[i, k] += _blocksInfo[i, k - 1];
                                _gameScore += _blocksInfo[i, k];
                                _blocksInfo[i, k] += 20000;
                                _blocksInfo[i, k - 1] = 0;
                                _isMove = true;
                                _tempNumberBlocks[(i * 4 + j % 4) - 1].AddMoveCount();
                            }
                        }
                    }
                }
                else if (_blocksInfo[i, j] == _blocksInfo[i, j - 1])
                {
                    _blocksInfo[i, j] += _blocksInfo[i, j - 1];
                    _gameScore += _blocksInfo[i, j];
                    _blocksInfo[i, j] += 10000;
                    _blocksInfo[i, j - 1] = 0;
                    _isMove = true;
                    _tempNumberBlocks[(i * 4 + j % 4) - 1].AddMoveCount();
                }
            }
        }
    }

    private void SubtractTenThousand()
    {
        for (int i = 0; i < MAX_BLOCK_COUNT; i++)
        {
            if (10000 < _blocksInfo[i / 4, i % 4])
            {
                _blocksInfo[i / 4, i % 4] %= 10000;
            }
        }
    }
    private void SumNumberBlocks()
    {
        for (int i = 0; i < MAX_BLOCK_COUNT; i++)
        {
            _numberBlocks[i].Number = _blocksInfo[i / 4, i % 4];
            _numberBlocks[i].ChangeImage(_numberBlocks[i].Number);
        }
    }
    private void CreateNumberBlock() // 숫자 커지면 나타나는 숫자도 커지기
    {
        if (_checkCreateNumberBlock == true)
        {
            // 비어있는자리찾기
            int random = Random.Range(0, 16);
            while (_blocksInfo[random / 4, random % 4] != 0)
            {
                random = Random.Range(0, 16);
            }

            // 생성될 숫자 구하기
            // 진행된 점수 비율에 따라 생성될 숫자 크기 올리기
            switch (Random.Range(1, 5))
            {
                case 1: // 4는 20%로 생성
                    {
                        _blocksInfo[random / 4, random % 4] = 4;
                        _numberBlocks[random].Number = _blocksInfo[random / 4, random % 4];
                        _numberBlocks[random].ChangeImage(_numberBlocks[random].Number);
                    }
                    break;
                default:
                    {
                        _blocksInfo[random / 4, random % 4] = 2;
                        _numberBlocks[random].Number = _blocksInfo[random / 4, random % 4];
                        _numberBlocks[random].ChangeImage(_numberBlocks[random].Number);
                    }
                    break;
            }
            _checkCreateNumberBlock = false;
            _isMove = false;
        }
    }
    private void CheckCreateNumberBlock()
    {
        int count = 0;
        if (_isMove == true)
        //이동을 한 경우에만 생성 여부 체크
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (_blocksInfo[i, j] != 0)
                    {
                        count++;
                    }
                }
            }
            if (count == 16)
            {
                //game over
                _checkCreateNumberBlock = false;
                SetGame2048State(Game2048State.None);
                // 실패 팝업 띄우기
            }
            else //if (count < 16)
            {
                _checkCreateNumberBlock = true;
            }
        }
    }
}
