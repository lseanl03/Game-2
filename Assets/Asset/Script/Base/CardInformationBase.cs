using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardInformationBase : MonoBehaviour
{
    public bool onClick = false;
    public bool onBegin = false;
    public float timeCount;
    public float hideInfoTime;
    
    public string headerText;
    public string contentText;
    public Sprite cardSprite;
    public CardDisplay cardDisplay;
    public TooltipManager tooltipManager;

    private void Start()
    {
        timeCount = 0f;
        hideInfoTime = 1f;
        cardDisplay = GetComponent<CardDisplay>();
        tooltipManager = FindObjectOfType<TooltipManager>();
    }
    private void Update()
    {
        if (onClick)
        {
            GetInfo();
            tooltipManager.ShowTooltip();
            if (onBegin)
            {
                tooltipManager.HideTooltip();
            }
        }
    }
    public void GetInfo()
    {
        headerText = cardDisplay.cardName;
        contentText = cardDisplay.cardDescription;
        cardSprite = cardDisplay.cardImage.sprite;
        tooltipManager.SetUp(headerText,contentText,cardSprite);
    }
}
