using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ScoreText : MonoBehaviour
{
    private TextMeshProUGUI _tmpGUI;
    private int _tempScore = 0;
    private Object _childTMP = null;

    void Awake()
    {
        _childTMP = Util.FindChild<TextMeshProUGUI>(gameObject, "ScoreTMP", true);    
    }

    void Start()
    {
        _tmpGUI = _childTMP.GetOrAddComponent<TextMeshProUGUI>();
    }


    public void ScoreUpdate(int score)
    {
        if (score != _tempScore)
        {
            _tmpGUI.text = score.ToString();
            _tempScore = score;
        }
    }

}
