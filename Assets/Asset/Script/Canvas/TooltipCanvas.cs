using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipCanvas : CanvasBase
{
    public CharacterCardTooltip characterCardTooltip;
    public ActionCardTooltip actionCardTooltip;

    public void CharacterCardTooltipState(bool state)
    {
        characterCardTooltip.gameObject.SetActive(state);
    }
    public void ActionCardTooltipState(bool state)
    {
        actionCardTooltip.gameObject.SetActive(state);
    }
}
