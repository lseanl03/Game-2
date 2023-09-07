using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCharacterBattlePanel : PanelBase
{
    public TextMeshProUGUI actionCostText;
    public TextMeshProUGUI switchCardBattleText;
    public Button selectCardBattleButton;
    public GameObject actionCostObj;
    public void Start()
    {
        SetSwitchCardBattleText("Hãy chọn một nhân vật để xuất chiến");
        SetActionCostText(string.Empty);
        ActionCostState(false);
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
                        SetSwitchCardBattleText("Chọn nhân vật xuất chiến");
                    }
                    else if (characterCardDragHovers[i].isSelecting)
                    {
                        if (!gamePlayManager.playerSelectedCharacterBattleInitial)
                        {
                            gamePlayManager.playerSelectedCharacterBattleInitial = true;
                            SetActionCostText(gamePlayManager.battleCardSwitchCost.ToString());
                            ActionCostState(true);
                        }
                        else
                        {
                            if (gamePlayManager.playerCanSwitchCharacterDying)
                            {
                                gamePlayManager.UpdateGameState(GamePlayState.ActionPhase);
                            }
                            else
                            {
                                if (playerManager.currentActionPoint >= gamePlayManager.battleCardSwitchCost)
                                {
                                    playerManager.ConsumeActionPoint(gamePlayManager.battleCardSwitchCost);
                                    if (!gamePlayManager.enemyEndingRound)
                                    {
                                        gamePlayManager.UpdateTurnState(TurnState.EnemyTurn);
                                    }
                                }
                                else
                                {
                                    notificationManager.SetNewNotification("Your don't have enough action points");
                                    return;
                                }
                            }
                        }
                        characterCardDragHovers[i].HandleCardSelecting();
                        PanelState(false);
                        uiManager.HideTooltip();
                    }
                    else if (characterCardDragHovers[i].isSelected)
                    {
                        if (playerManager.currentActionPoint >= gamePlayManager.battleCardSwitchCost)
                        {
                            characterCardDragHovers[i].HandleCardSelected();
                            PanelState(false);
                        }
                    }
                }
                if (gamePlayManager.gamePlayCanvas.playerActionCardField.isZooming)
                {
                    gamePlayManager.gamePlayCanvas.playerActionCardField.ZoomState(false);
                }
            }
        }
    }
    public void SetSwitchCardBattleText(string text)
    {
        switchCardBattleText.text = text;
    }
    public void SetActionCostText(string text)
    {
        actionCostText.text = text;
    }
    public void ActionCostState(bool state)
    {
        actionCostObj.SetActive(state);
    }
}
