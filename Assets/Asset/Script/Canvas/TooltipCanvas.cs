using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipCanvas : CanvasBase
{
    public Image backImage;
    public Image actionCardImage;
    public Image characterCardImage;

    public TextMeshProUGUI manaText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public TooltipController tooltipController;

    public CharacterCardData currentCharacterCardData;
    public ActionCardData currentActionCardData;

    public void Start()
    {
        tooltipController.StateObj(false);
    }
    public void GetCharacterCardInfo(CharacterCardData characterCardData)
    {
        currentCharacterCardData = characterCardData;

        characterCardImage.sprite = currentCharacterCardData.cardSprite;
        healthText.text = currentCharacterCardData.maxHealth.ToString();
        nameText.text = currentCharacterCardData.characterName;
        descriptionText.text = currentCharacterCardData.description;

        CharacterCardImageState(true);
        ActionCardImageState(false);
    }
    public void GetActionCardInfo(ActionCardData actionCardData)
    {
        currentActionCardData = actionCardData;

        backImage.color = currentActionCardData.actionCard.colorRarity;
        actionCardImage.sprite = currentActionCardData.cardSprite;
        healthText.text = currentActionCardData.cardCost.ToString();
        nameText.text = currentActionCardData.cardName;
        descriptionText.text = currentActionCardData.cardDescription;

        ActionCardImageState(true);
        CharacterCardImageState(false);
    }
    public void ActionCardImageState(bool state)
    {
        actionCardImage.gameObject.SetActive(state);
    }
    public void CharacterCardImageState(bool state)
    {
        characterCardImage.gameObject.SetActive(state);
    }
}
