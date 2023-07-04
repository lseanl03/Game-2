using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public bool showTooltip;
    public string headerText;
    public string contentText;
    public Sprite cardSprite;
    public Image cardImage;

    public GameObject backGroundImageObj;
    public GameObject cardImageObj;

    public TooltipController tooltipController;

    public InformationData informationData;

    public static TooltipManager instance;

    public GameObject backgroundClick;
    private void Start()
    {
        backgroundClick.SetActive(false);
        HideTooltip();
    }
    private void Update()
    {
        BackgroundClickActive();
    }
    public void SetUp(string headerT, string contentT, Sprite cardSprite)
    {
        this.headerText = headerT;
        this.contentText = contentT;
        this.cardSprite = cardSprite;
        cardImage.sprite = cardSprite;
        tooltipController.SetText(this.headerText, this.contentText);
        tooltipController.SetCardImage(this.cardImage);
    }
    public void ShowTooltip()
    {
        showTooltip = true;
        backGroundImageObj.SetActive(true);
        cardImageObj.SetActive(true);
    }
    public void HideTooltip()
    {
        showTooltip = false;
        backGroundImageObj.SetActive(false);
        cardImageObj.SetActive(false);
    }
    public void BackgroundClickActive()
    {
        if(backGroundImageObj.activeSelf && cardImageObj.activeSelf)
        {
            backgroundClick.SetActive(true);
        }
        else
        {
            backgroundClick.SetActive(false);
        }
    }
}
