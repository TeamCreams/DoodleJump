using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_Vibration : UI_Base
{
    private enum Images
    {
        Handle
    }
    private Animator _animator;
    private bool _isOn = true; //이거를 저장값을 가져오도록
    
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        _animator = this.GetOrAddComponent<Animator>();
        BindImages(typeof(Images));
        GetImage((int)Images.Handle).gameObject.BindEvent(OnClick_SwitchButton, EUIEvent.Click);
        _isOn = Managers.Game.SettingInfo.VibrationIsOn;
        _animator.SetBool("IsSwitchOn", _isOn);
        return true;
    }
    private void OnClick_SwitchButton(PointerEventData eventData)
    {
        _isOn = !_isOn;
        Managers.Game.SettingInfo.VibrationIsOn = _isOn;
        _animator.SetBool("IsSwitchOn", _isOn);
        Debug.Log($"///OnClick_SwitchButton(Managers.Game.SettingInfo.VibrationIsOn) : {Managers.Game.SettingInfo.VibrationIsOn}");
        //Vibration기능 추가
    }
}
