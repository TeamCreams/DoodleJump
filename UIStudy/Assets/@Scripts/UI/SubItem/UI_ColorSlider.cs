using Assets.HeroEditor4D.SimpleColorPicker.Scripts;
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

    enum LegacyTexts
    {
        Input_Text,

    }

    private int MaxValue = 0;


    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindSliders(typeof(Sliders));
        BindLegacyTexts(typeof(LegacyTexts));
        return true;
    }

    private void OnEnable()
    {
        //_colorPicker = this.transform.parent.gameObject.GetComponent<UI_ColorPicker>();
        // FIX : parent에 있는것이 아니라, parent의 parent의 parent에 있는것으로 추정됨.
        //      이런 경우에는 그냥 심플하게 생각하는게 좋음 아래처럼

        //_colorPicker = Managers.UI.GetSceneUI<UI_ChooseCharacterScene>().ColorPicker;
        Debug.Assert(_colorPicker != null, $"{nameof(_colorPicker)} is null");
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
        GetLegacyText((int)LegacyTexts.Input_Text).text = Mathf.RoundToInt(value * MaxValue).ToString();
    }

    /// <summary>
    /// Called when slider value changed.
    /// </summary>
    public void OnValueChanged(float value)
    {
        if (_colorPicker != null && _colorPicker.Locked) return;

        GetLegacyText((int)LegacyTexts.Input_Text).text = Mathf.RoundToInt(value * MaxValue).ToString();
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
            GetLegacyText((int)LegacyTexts.Input_Text).text = "";
        }
        else
        {
            var integer = Mathf.Min(int.Parse(value), MaxValue);

            GetLegacyText((int)LegacyTexts.Input_Text).text = integer.ToString();
            GetSlider((int)Sliders.Slider).value = (float)integer / MaxValue;
            _colorPicker.OnSliderChanged();
        }
    }
}
