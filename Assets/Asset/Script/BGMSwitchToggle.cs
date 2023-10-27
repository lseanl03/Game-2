using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSwitchToggle : SwitchToggle
{
    public void Start()
    {
        if (PlayerPrefs.HasKey("BGMState"))
        {
            bool bgmIsOn = PlayerPrefs.GetInt("BGMState") == 1;
            OnSwitchNoAni(bgmIsOn);
        }
        else
        {
            OnSwitchNoAni(true);
        }
    }
    protected override void OnSwitch(bool isOn)
    {
        base.OnSwitch(isOn);
        PlayerPrefs.SetInt("BGMState", isOn ? 1 : 0);
    }
}
