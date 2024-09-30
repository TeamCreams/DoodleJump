using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using static Define;
using UnityEngine.EventSystems;

public class UI_ColorPicker : UI_Base
{
    private Color Color;
    private EColorMode eColorMode;
    //public Image Gradient;
    //public RectTransform RectTransform;
    //public Slider Hue;
    //private UI_ColorSlider[] colorSliders;

    // FIX : 정적배열보다는 List로 관리하는게 대부분의 상황에서 좋음
    private List<UI_ColorSlider> colorSliders = new List<UI_ColorSlider>();

    //public UI_ColorSlider R, G, B, H, S, V, A;
    //public InputField Hex;
    //public Image[] CompareLook; // [0] is old color, [1] is new color.
    //public Image TransparencyLook;
    //public Text Mode;
    //public GameObject RgbSliders;
    //public GameObject HsvSliders;
    private bool _locked;

    public bool Locked
    {
        get => _locked;
        private set
        {
            _locked = value;
        }
    }

    enum Images
    {
        Gradient,
        Current, //is old color
        New, //is new color
        Sample, // TransparencyLook
        Cancel,
        Confirm,
    }
    enum Sliders
    {
        Hue,
    }
    enum LegacyInputFields
    {
        Hex
    }
    enum LegacyTexts
    {
        Mode_Text,
    }
    enum GameObjects
    {
        R,
        G,
        B,
        H,
        S,
        V,
        A,
        RGB, //RgbSliders,      // FIX : 대소문자 구분으로 못찾음
        HSV, //HsvSliders       // FIX : 대소문자 구분으로 못찾음
    }

    [HideInInspector] public Texture2D Texture;

    public Action<Color> OnColorChanged;

    /// <summary>
    /// Called on app start if script is enabled.
    /// </summary>
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }


        BindImages(typeof(Images));
        BindSliders(typeof(Sliders));
        BindLegacyInputFields(typeof(LegacyInputFields));
        BindLegacyTexts(typeof(LegacyTexts));
        BindObjects(typeof(GameObjects));

        Texture = new Texture2D(128, 128) { filterMode = FilterMode.Point };
        GetImage((int)Images.Gradient).sprite = Sprite.Create(Texture, new Rect(0f, 0f, Texture.width, Texture.height), new Vector2(0.5f, 0.5f), 100f);

        Debug.Log("0987654321234567890ColorPicker");
        int i = 0;
        foreach (var slider in Enum.GetValues(typeof(GameObjects)))
        {
            var gameObject = Get<GameObject>((int)slider);
            // FIX : 이렇게 현재 못찾은 애가 누구인지 로그찍어보기
            if (gameObject == null)
            {
                Debug.LogError($"{(GameObjects)slider} 을 못찾음");
            }
            var uiColorSlider = gameObject.GetComponent<UI_ColorSlider>();

            if (uiColorSlider != null)
            {
                colorSliders.Add(uiColorSlider);//gameObject.GetOrAddComponent<UI_ColorSlider>();
                i++;
            }
        }


        GetImage((int)Images.Cancel).gameObject.BindEvent(OnClick_Cancle, Define.EUIEvent.Click);

        GetImage((int)Images.Confirm).gameObject.BindEvent(OnClick_Confirm, Define.EUIEvent.Click);

        //        this.gameObject.SetActive(false);


        return true;
    }

    public void OnEnable()
    {
        SetColor(Color);
        GetImage((int)Images.Current).color = Color;
    }

    /// <summary>
    /// Called when Select button pressed
    /// </summary>
    public void Select()
    {
        GetImage((int)Images.Current).color = Color;
        //CompareLook[0].color = Color;
        Debug.LogFormat("Color selected: {0}", Color);
    }

    /// <summary>
    /// Set color picker RGB color.
    /// </summary>
    public void SetColor(Color color, bool picker = true, bool sliders = true, bool hex = true, bool hue = true)
    {
        Color.RGBToHSV(color, out var h, out var s, out var v);
        SetColor(s > 0 ? h : colorSliders[3].Value, s, v, color.a, picker, sliders, hex, hue);
    }

    /// <summary>
    /// Set color picker HSV color.
    /// </summary>
    public void SetColor(float h, float s, float v, float a, bool picker = true, bool sliders = true, bool hex = true, bool hue = true)
    {
        var color = Color.HSVToRGB(h, s, v);

        color.a = a;

        Color = GetImage((int)Images.Sample).color = GetImage((int)Images.New).color = color;
        Locked = true;

        if (sliders || eColorMode == EColorMode.Hsv)
        {

            colorSliders[0].Set(Color.r);
            colorSliders[1].Set(Color.g);
            colorSliders[2].Set(Color.b);
        }

        if (sliders || eColorMode == EColorMode.Rgb)
        {
            colorSliders[3].Set(h);
            colorSliders[4].Set(s);
            colorSliders[5].Set(v);
        }

        colorSliders[6].Set(Color.a);

        
        if (hue) GetSlider((int)Sliders.Hue).value = h;
        if (hex) GetLegacyInputField((int)LegacyInputFields.Hex).text = ColorUtility.ToHtmlStringRGBA(Color);

        Locked = false;
        UpdateGradient();
        OnColorChanged?.Invoke(Color);
    }

    /// <summary>
    /// Called when HUE changed.
    /// </summary>
    public void OnHueChanged(float value)
    {
        if (Locked) return;

        Color.RGBToHSV(Color, out var h, out var s, out var v);
        h = value;
        SetColor(h, s, v, colorSliders[6].Value, hue: false);
    }

    /// <summary>
    /// Called when slider changed.
    /// </summary>
    public void OnSliderChanged()
    {
        if (Locked) return;

        if (eColorMode == EColorMode.Rgb)
        {
            SetColor(new Color(colorSliders[0].Value, colorSliders[1].Value, colorSliders[2].Value, colorSliders[6].Value), sliders: false);
        }
        else
        {
            SetColor(colorSliders[3].Value, colorSliders[4].Value, colorSliders[5].Value, colorSliders[6].Value, sliders: false);
        }
    }

    /// <summary>
    /// Called when HEX code changed.
    /// </summary>
    public void OnHexValueChanged(string value)
    {
        if (Locked) return;

        value = Regex.Replace(value.ToUpper(), "[^0-9A-F]", "");
        GetLegacyInputField((int)LegacyInputFields.Hex).text = value;

        if (ColorUtility.TryParseHtmlString("#" + value, out var color))
        {
            SetColor(color, hex: false);
        }
    }

    /// <summary>
    /// Switch mode RGB/HSV.
    /// </summary>
    public void SwitchMode()
    {
        eColorMode = eColorMode == EColorMode.Rgb ? EColorMode.Hsv : EColorMode.Rgb;
        SetMode(eColorMode);
    }

    /// <summary>
    /// Set mode RGB/HSV.
    /// </summary>
    public void SetMode(EColorMode mode)
    {
        GetObject((int)GameObjects.RGB).SetActive(mode == EColorMode.Rgb);
        GetObject((int)GameObjects.HSV).SetActive(mode == EColorMode.Hsv);
        GetText((int)LegacyTexts.Mode_Text).text = mode == EColorMode.Rgb ? "HSV" : "RGB";
    }

    private void UpdateGradient()
    {
        var pixels = new List<Color>();

        for (var y = 0; y < Texture.height; y++)
        {
            for (var x = 0; x < Texture.width; x++)
            {
                pixels.Add(Color.HSVToRGB(GetSlider((int)Sliders.Hue).value, (float)x / Texture.width, (float)y / Texture.height));
            }
        }

        Texture.SetPixels(pixels.ToArray());
        Texture.Apply();
    }

    private void OnClick_Cancle(PointerEventData eventData)
    {
        this.gameObject.SetActive(false);
    }

    private void OnClick_Confirm(PointerEventData eventData)
    {

    }
}

