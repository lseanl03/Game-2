using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionCardTooltip : MonoBehaviour
{
    [Header("Action Card")]
    public Image actionBackImage;
    public Image actionCardImage;

    public TextMeshProUGUI manaText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    [Header("Data")]
    public ActionCardData currentActionCardData;
    public void GetActionCardInfo(ActionCardData actionCardData)
    {
        currentActionCardData = actionCardData;

        actionCardImage.sprite = currentActionCardData.cardSprite;
        actionBackImage.color = currentActionCardData.actionCard.colorRarity;
        nameText.text = currentActionCardData.cardName;
        descriptionText.text = currentActionCardData.cardDescription;
    }
}
