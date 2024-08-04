using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ScoreText : MonoBehaviour
{

    private TMP_Text _text;
    private TextMeshProUGUI _tmpGUI;

    private NumberBlockController _blockController;
    public TMP_Text GameScoreText
    {
        get { return _text; }
        set { _text = value; ; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _blockController = FindObjectOfType<NumberBlockController>();
        _tmpGUI = this.GetOrAddComponent<TextMeshProUGUI>();
        _tmpGUI.text = _blockController.GameScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
