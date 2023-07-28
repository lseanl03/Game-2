using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionalToolCanvas : CanvasBase
{
    public CardImage cardImage;
    public CardDescription cardDescription;
    public CharacterCard currentCharacterCard;
    public ActionCard currentActionCard;
    public void GetCharacterCardInfo(CharacterCard characterCard)
    {
        currentCharacterCard = characterCard;

        cardImage.cardImage.sprite = characterCard.cardImage.sprite;
        cardImage.healthText.text = characterCard.healthText.text;
        cardDescription.descriptionCardText.text = characterCard.descriptionText.text;
        cardDescription.nameCard.text = characterCard.nameText.text;
    }
    public void GetActionCardInfo(ActionCard actionCard)
    {
        currentActionCard = actionCard;

        cardImage.cardImage.sprite = actionCard.cardImage.sprite;
        cardImage.healthText.text = actionCard.manaText.text;
        cardDescription.descriptionCardText.text = actionCard.actionCardData.cardDescription;
        cardDescription.nameCard.text = actionCard.actionCardData.cardName;
        cardDescription.quantityAddedText.text = actionCard.quantitySelected + " / " + actionCard.quantity.text;
    }
}
