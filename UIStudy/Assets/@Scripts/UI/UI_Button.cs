using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour
{
    [SerializeField]
    Button _button;

    private void OnValidate()
    {

        _button = this.GetComponent<Button>();
    }

    private void Reset()
    {

        _button = this.GetComponent<Button>();
    }

}
