using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameScene : MonoBehaviour
{
    // Score
    public PlayerController PlayerController { get; private set; }

    [SerializeField]
    private TextMeshProUGUI _scoreText;

    public void Awake()
    {
        _scoreText = Util.FindChildWithPath<TextMeshProUGUI>("@GameUI/ScoreText");
        _scoreText.text = $"{100} Á¡";

        PlayerController = Util.FindChildWithPath<PlayerController>("@playerChracter");
    }
}
