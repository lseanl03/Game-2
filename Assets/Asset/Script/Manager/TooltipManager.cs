using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public TooltipCanvas tooltipCanvas;
    public static TooltipManager instance;
    protected UIManager uiManager => UIManager.instance; 
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public void ShowCharacterCardTooltip(CharacterCard characterCard)
    {
        tooltipCanvas.CanvasState(true);
        tooltipCanvas.CharacterCardTooltipState(true);
        tooltipCanvas.ActionCardTooltipState(false);
        tooltipCanvas.characterCardTooltip.GetCharacterCardInfo(characterCard);
    }
    public void ShowActionCardTooltip(ActionCard actionCard)
    {
        if (!uiManager.battleCanvas.playCardPanel.isShowingCardInfo)
        {
            tooltipCanvas.CanvasState(true);
            tooltipCanvas.ActionCardTooltipState(true);
            tooltipCanvas.CharacterCardTooltipState(false);
            tooltipCanvas.actionCardTooltip.GetActionCardInfo(actionCard);
        }
    }
}
