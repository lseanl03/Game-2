using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCardBattlePanel : PanelBase
{
    public TextMeshProUGUI costManaText;
    public TextMeshProUGUI selectCardBattleText;
    public Button selectCardBattleButton;
    public CharacterCardDragHover[] characterCardDragHovers;
    public void Start()
    {
        SetText("Hãy chọn một nhân vật để xuất chiến");
        SetCostText(string.Empty);
    }
    public void SelectCardBattle()
    {
        Debug.Log("button");
        if(gamePlayManager != null)
        {
            PlayerCharacterCardField playerCharacterCardField = gamePlayManager.gamePlayCanvas.playerCharacterCardField;
            if (playerCharacterCardField != null)
            {
                characterCardDragHovers = playerCharacterCardField.GetComponentsInChildren<CharacterCardDragHover>();
                for(int i= 0; i < characterCardDragHovers.Length; i++)
                {
                    if (!characterCardDragHovers[i].isSelecting && !characterCardDragHovers[i].isSelected)
                    {
                        SetText("Chọn nhân vật xuất chiến");
                    }
                    else if (characterCardDragHovers[i].isSelecting)
                    {
                        SetCostText(gamePlayManager.characterCardSwitchCost.ToString());
                        characterCardDragHovers[i].HandleCardSelecting();
                        PanelState(false);
                    }
                    else if (characterCardDragHovers[i].isSelected)
                    {
                        characterCardDragHovers[i].isSelected = false;
                        characterCardDragHovers[i].transform.localPosition = new Vector2(characterCardDragHovers[i].transform.localPosition.x, 0f);
                        PanelState(false);
                    }
                }
            }
        }
    }
    public void SetText(string text)
    {
        selectCardBattleText.text = text;
    }
    public void SetCostText(string text)
    {
        costManaText.text = text;
    }
}
