using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Game2048 : MonoBehaviour
{
    private List<GameObject> _numberBlocks = new List<GameObject>(); // ºí·Ï

    public IReadOnlyList<GameObject> NumberBlocks => _numberBlocks;

    private NumberBlockController _numberBlockController;
    private ScoreText _scoreText;

    private void Awake()
    {
        //NumberBlockController
        GameObject controllerObject = new GameObject("@NumberBlockController");
        _numberBlockController = controllerObject.AddComponent<NumberBlockController>();

        //ScoreText
        var scorePanel = Managers.Resource.Instantiate("ScorePanel");
        scorePanel.transform.position = new Vector2(-2, 15);
        _scoreText = scorePanel.GetOrAddComponent<ScoreText>();

        //NumberBlock
        for (int i = 0; i < 16; i++)
        {
            var instance = Managers.Resource.Instantiate("NumberBlock");
            float x = -4 + (i / 4) * 4f;
            float y = -4 + (i % 4) * 4f;
            instance.transform.position = new Vector2(x, y);
            _numberBlocks.Add(instance);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        _numberBlockController.Init(_numberBlocks);
    }

    // Update is called once per frame
    void Update()
    {
        _numberBlockController.UpdateFunc();

        _scoreText.ScoreUpdate(_numberBlockController.GameScore);
    }

}
