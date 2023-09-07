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

    public TextMeshProUGUI actionPointCostText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    [Header("Data")]
    public ActionCard currentActionCard;
    public void GetActionCardInfo(ActionCard actionCard)
    {
        currentActionCard = actionCard;

        actionCardImage.sprite = currentActionCard.actionCardData.cardSprite;
        actionBackImage.color = currentActionCard.actionCardData.actionCard.colorRarity;
        nameText.text = currentActionCard.actionCardData.cardName;
        descriptionText.text = currentActionCard.actionCardData.cardDescription;
        actionPointCostText.text = currentActionCard.actionCost.ToString();
    }
}
