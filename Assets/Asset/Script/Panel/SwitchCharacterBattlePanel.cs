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
    public void SelectCardBattle()
    {
        if(gamePlayManager != null)
        {
            bool consumedActionPoint = false;
            PlayerCharacterCardField playerCharacterCardField = gamePlayManager.gamePlayCanvas.playerCharacterCardField;
            CharacterCardDragHover[] characterCardDragHovers = playerCharacterCardField.GetComponentsInChildren<CharacterCardDragHover>();
            for (int i = 0; i < characterCardDragHovers.Length; i++)
            {
                if (!characterCardDragHovers[i].isSelecting && !characterCardDragHovers[i].isSelected)
                {
                    SetSwitchCardBattleText("Chọn nhân vật xuất chiến");
                }
                else if (characterCardDragHovers[i].isSelected)
                {
                    if (consumedActionPoint)
                    {
                        characterCardDragHovers[i].HandleCardSelected();
                        PanelState(false);
                    }
                    else
                    {
                        if(playerManager.currentActionPoint >= gamePlayManager.battleCardSwitchCost)
                        {
                            characterCardDragHovers[i].HandleCardSelected();
                            PanelState(false);
                        }
                    }
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
                        if (gamePlayManager.playerCanSwitchCharacterDying && gamePlayManager.actionPhase)
                        {
                            gamePlayManager.UpdateGameState(GamePlayState.ActionPhase);
                        }
                        else
                        {
                            if (playerManager.currentActionPoint >= gamePlayManager.battleCardSwitchCost)
                            {
                                playerManager.ConsumeActionPoint(gamePlayManager.battleCardSwitchCost);
                                consumedActionPoint = true;
                                CheckActionFastSwitchCharacter();
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
            }
            if (gamePlayManager.gamePlayCanvas.playerActionCardField.isZooming)
            {
                gamePlayManager.gamePlayCanvas.playerActionCardField.ZoomState(false);
            }
        }
    }
    public void CheckActionFastSwitchCharacter()
    {
        if (!gamePlayManager.enemyEndingRound)
        {
            if (gamePlayManager.GetSupportCardUsing(SupportActionSkillType.FastSwitchCharacter) == null)
                gamePlayManager.UpdateTurnState(TurnState.EnemyTurn);
            else
            {
                SupportCard supportCard = gamePlayManager.GetSupportCardUsing(SupportActionSkillType.FastSwitchCharacter);
                SupportActionSkill supportActionSkill = supportCard.supportCardData.actionCard.supportActionSkill;
                gamePlayManager.UseSupportCard(supportActionSkill.actionStartPhase, true);
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
