using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // GameOver
    private GameObject _gameOver;
    private GameObject _gameOverImages;
    private Vector2 _startPos = Vector2.zero;
    private Vector2 _endPos = Vector2.zero;

    // Restart
    private Button _startButton;
    
    // Score
    private GameObject _player;
    private int _score;
    private Text _scoreText;

    public void SettingUI()
    {
        // Restart
        GameObject _startButtonObject = GameObject.Find("StartButton");
        _startButton = _startButtonObject.GetComponent<Button>();
        _startButton.GetComponent<Button>().onClick.AddListener(Restart);

        // GameOver
        _gameOver = GameObject.FindWithTag("GameOverUI");
        _gameOverImages = GameObject.FindWithTag("GameOverImages");

        _startPos = _gameOverImages.transform.position;
        _endPos = _startPos + new Vector2(0, 300);
        _gameOver.SetActive(false);

        // Score
        _player = GameObject.FindWithTag("Player");
        GameObject _scoreTextObject = GameObject.Find("ScoreText");
        _scoreText = _scoreTextObject.GetComponent<Text>(); 
    }

    public IEnumerator UpUI()
    {
        _gameOver.SetActive(true);
        float _time = 0;
        while (_time < 0.5f)
        {
            _time += Time.deltaTime;
            float t = Mathf.Clamp01(_time / 0.5f);
            _gameOverImages.transform.position = Vector3.Lerp(_startPos, _endPos, t);
            if(_endPos.y <= _gameOverImages.transform.position.y) // ���� �غ���
            {
                break;
            }
            yield return null;
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Score()
    {
        _score = (int)(_player.transform.position.y) * 5;
        _scoreText.text = _score.ToString();
    }
}
