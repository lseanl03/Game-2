using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXSwitchToggle : SwitchToggle
{
    public void Start()
    {
        if (PlayerPrefs.HasKey("SFXState"))
        {
            bool sfxIsOn = PlayerPrefs.GetInt("SFXState") == 1;
            OnSwitchNoAni(sfxIsOn);
        }
        else
        {
            OnSwitchNoAni(true);
        }
    }
    protected override void OnSwitch(bool isOn)
    {
        base.OnSwitch(isOn);
        PlayerPrefs.SetInt("SFXState", isOn ? 1 : 0);
    }
}
