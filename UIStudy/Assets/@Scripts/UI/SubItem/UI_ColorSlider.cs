using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_ColorSlider : UI_Base
{
    public UI_ColorPicker _colorPicker;
    enum Sliders
    {
        Slider,
    }

    enum Texts
    {
        Input_Text,

    }

    private int MaxValue;


    protected override void Init()
    {
        base.Init();
        BindSlider(typeof(Sliders));
        BindTexts(typeof(Texts));
        _colorPicker = this.transform.parent.gameObject.GetComponent<UI_ColorPicker>();
        if(_colorPicker == null)
        {
            Debug.Log("is nullllllllllllllllllllllllllllllllllllllllllllll");
        }
    }

    /// <summary>
    /// Quick access.
    /// </summary>
    public float Value { get { return GetSlider((int)Sliders.Slider).value; } }

    /// <summary>
    /// Set lider value.
    /// </summary>
    public void Set(float value)
    {
        GetSlider((int)Sliders.Slider).value = value;
        GetText((int)Texts.Input_Text).text = Mathf.RoundToInt(value * MaxValue).ToString();
    }

    /// <summary>
    /// Called when slider value changed.
    /// </summary>
    public void OnValueChanged(float value)
    {
        if (_colorPicker.Locked) return;

        GetText((int)Texts.Input_Text).text = Mathf.RoundToInt(value * MaxValue).ToString();
        _colorPicker.OnSliderChanged();
    }

    /// <summary>
    /// Called when input field value changed.
    /// </summary>
    public void OnValueChanged(string value)
    {
        if (_colorPicker.Locked) return;

        value = value.Replace("-", null);

        if (value == "")
        {
            GetText((int)Texts.Input_Text).text = "";
        }
        else
        {
            var integer = Mathf.Min(int.Parse(value), MaxValue);

            GetText((int)Texts.Input_Text).text = integer.ToString();
            GetSlider((int)Sliders.Slider).value = (float)integer / MaxValue;
            _colorPicker.OnSliderChanged();
        }
    }
}
