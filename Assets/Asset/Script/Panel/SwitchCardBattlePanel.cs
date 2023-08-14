using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCardBattlePanel : PanelBase
{
    public TextMeshProUGUI costManaText;
    public TextMeshProUGUI switchCardBattleText;

    public Button selectCardBattleButton;

    public GameObject manaCostObj;
    public void Start()
    {
        SetText("Hãy chọn một nhân vật để xuất chiến");
        SetCostText(string.Empty);
        ManaCostState(false);
    }
    public void SelectCardBattle()
    {
        if(gamePlayManager != null)
        {
            PlayerCharacterCardField playerCharacterCardField = gamePlayManager.gamePlayCanvas.playerCharacterCardField;
            if (playerCharacterCardField != null)
            {
                CharacterCardDragHover[] characterCardDragHovers = playerCharacterCardField.GetComponentsInChildren<CharacterCardDragHover>();
                for(int i= 0; i < characterCardDragHovers.Length; i++)
                {
                    if (!characterCardDragHovers[i].isSelecting && !characterCardDragHovers[i].isSelected)
                    {
                        SetText("Chọn nhân vật xuất chiến");
                    }
                    else if (characterCardDragHovers[i].isSelecting)
                    {
                        if (!gamePlayManager.playerSelectedCardBattleInitial)
                        {
                            gamePlayManager.playerSelectedCardBattleInitial = true;
                            SetCostText(gamePlayManager.battleCardSwitchCost.ToString());
                            ManaCostState(true);

                        }
                        else
                        {
                            if (playerManager.currentActionPoint >= gamePlayManager.battleCardSwitchCost)
                            {
                                playerManager.ConsumeActionPoint(gamePlayManager.battleCardSwitchCost);
                                gamePlayManager.UpdateTurnState(TurnState.EnemyTurn);
                            }
                            else
                            {
                                notificationManager.SetNewNotification("You don't have enough action points");
                                return;
                            }
                        }
                        characterCardDragHovers[i].HandleCardSelecting();
                        PanelState(false);
                        uiManager.HideTooltip();

                        if (gamePlayManager.gamePlayCanvas.playerActionCardField.isZooming)
                        {
                            gamePlayManager.gamePlayCanvas.playerActionCardField.ZoomState(false);
                        }
                    }
                    else if (characterCardDragHovers[i].isSelected)
                    {
                        characterCardDragHovers[i].HandleCardSelected();
                        PanelState(false);
                    }
                }
            }
        }
    }
    public void SetText(string text)
    {
        switchCardBattleText.text = text;
    }
    public void SetCostText(string text)
    {
        costManaText.text = text;
    }
    public void ManaCostState(bool state)
    {
        manaCostObj.SetActive(state);
    }
}
