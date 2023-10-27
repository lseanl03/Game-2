using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    public RectTransform handleRectTransform;
    public Color backgroundActiveColor;
    public Color handleActiveColor;

    private Vector2 handlePosition;
    private Image backgroundImage;
    private Image handleImage;
    public Toggle toggle;
    private Color backgroundDefaultColor;
    private Color handleDefaultColor;
    public void Awake()
    {
        toggle = GetComponent<Toggle>();
        backgroundImage = handleRectTransform.parent.GetComponent<Image>();
        handleImage = handleRectTransform.GetComponent<Image>();

        backgroundDefaultColor = backgroundImage.color;
        handleDefaultColor = handleImage.color;

        
        handlePosition = handleRectTransform.anchoredPosition;
        toggle.onValueChanged.AddListener(OnSwitch);
    }
    protected virtual void OnSwitch(bool isOn)
    {
        toggle.isOn = isOn;
        handleRectTransform.DOAnchorPos( isOn ? handlePosition : handlePosition*-1, 0.25f).SetEase(Ease.InOutBack);
        backgroundImage.DOColor(isOn ? backgroundActiveColor : backgroundDefaultColor, 0.25f);
        handleImage.DOColor(isOn ? handleActiveColor: handleDefaultColor, 0.25f);
    }
    protected virtual void OnSwitchNoAni(bool isOn)
    {
        toggle.isOn = isOn;
        handleRectTransform.anchoredPosition = isOn ? handlePosition : handlePosition * -1;
        backgroundImage.color =  isOn ? backgroundActiveColor : backgroundDefaultColor;
        handleImage.color = isOn ? handleActiveColor : handleDefaultColor;
    }
    public void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}
