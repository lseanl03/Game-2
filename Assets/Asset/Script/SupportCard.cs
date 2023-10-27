using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SupportCard : CardBase
{
    public bool canDestroy = false;
    [Header("Data")]
    public int countOfActions;
    public int maxCountOfAction;
    public ActionCardData supportCardData;

    [Header("Image")]
    public Sprite cardSprite;
    public Image cardImage;
    public Image backImage;

    [Header("Text")]
    public TextMeshProUGUI countOfActionsText;

    [Header("Component")]
    public CanvasGroup canvasGroup;

    public void GetCardData(ActionCardData actionCardData)
    {
        this.supportCardData = actionCardData;
        SetCardImage(supportCardData.cardSprite);
        countOfActions = supportCardData.actionCard.supportActionSkill.countOfActions;
        maxCountOfAction = supportCardData.actionCard.supportActionSkill.maxCountOfActions;
        SetActionCost();
    }
    public void SetActionCost()
    {
        countOfActionsText.text = countOfActions.ToString();
        countOfActionsText.transform.DOScale(1.5f, 0.25f).SetLoops(2, LoopType.Yoyo);
    }
    public void SetCardImage(Sprite sprite)
    {
        cardSprite = sprite;
        cardImage.sprite = cardSprite;
    }
    public void DoSupportAction(SupportActionSkill supportActionSkill,List<CharacterCard> targetList, bool player)
    {
        if (supportActionSkill.actionStartPhase == ActionStartPhase.Now)
        {
            canDestroy = true;
            switch (supportActionSkill.supportSkillType)
            {
                case SupportActionSkillType.FastSwitchCharacter:
                    CheckCountOfActions(supportActionSkill.supportSkillType);
                    break;
            }
        }
        else if (supportActionSkill.actionStartPhase == ActionStartPhase.StartRound || 
            supportActionSkill.actionStartPhase == ActionStartPhase.EndRound)
        {
            canDestroy = true;
            switch (supportActionSkill.supportSkillType)
            {
                case SupportActionSkillType.DrawActionCard:
                    if (player)
                    {
                        if (gamePlayManager.gamePlayCanvas.playerActionCardField.transform.childCount < 10)
                        {
                            CheckCountOfActions(supportActionSkill.supportSkillType);
                            gamePlayManager.gamePlayCanvas.DrawCardsUsingSupportCards(supportActionSkill.actionValue, player);
                        }
                    }
                    else
                    {
                        if (gamePlayManager.gamePlayCanvas.enemyActionCardField.transform.childCount < 10)
                        {
                            CheckCountOfActions(supportActionSkill.supportSkillType);
                            gamePlayManager.gamePlayCanvas.DrawCardsUsingSupportCards(supportActionSkill.actionValue, player);
                        }
                    }
                    break;
                case SupportActionSkillType.Healing:
                    foreach(CharacterCard characterCard in targetList)
                    {
                        if (characterCard.currentHealth == characterCard.characterCardData.maxHealth || 
                            characterCard.characterStats.isDead)
                            return;
                    }
                    CheckCountOfActions(supportActionSkill.supportSkillType);
                    HealingAction healingAction = new HealingAction();
                    healingAction.DoSupportAction(supportActionSkill, targetList);
                    break;
                case SupportActionSkillType.IncreaseActionPoint:
                    CheckCountOfActions(supportActionSkill.supportSkillType);
                    if (player) playerManager.RecoveryActionPoint(supportActionSkill.actionValue);
                    else enemyManager.RecoveryActionPoint(supportActionSkill.actionValue);
                    break;
                case SupportActionSkillType.AccumulateAndDrawActionCard:
                    CheckCountOfActions(supportActionSkill.supportSkillType);
                    if(countOfActions == maxCountOfAction)
                    {
                        if (player) gamePlayManager.gamePlayCanvas.DrawCardsUsingSupportCards(supportActionSkill.actionValue, player);
                        else gamePlayManager.gamePlayCanvas.DrawCardsUsingSupportCards(supportActionSkill.actionValue, player);
                    }
                    break;
            }
        }
        else if(supportActionSkill.actionStartPhase == ActionStartPhase.StartActionPhaseAndEndCheckPhase)
        {
            switch (supportActionSkill.supportSkillType)
            {
                case SupportActionSkillType.CollectActionPoints:
                    if (gamePlayManager.endPhase)
                    {
                        if (!canDestroy)
                        {
                            if (player)
                            {
                                countOfActions += playerManager.currentActionPoint;
                                if (countOfActions >= maxCountOfAction) countOfActions = maxCountOfAction;

                                playerManager.currentActionPoint -= countOfActions;
                                if (playerManager.currentActionPoint <= 0) playerManager.currentActionPoint = 0;
                            }
                            else
                            {
                                countOfActions += enemyManager.currentActionPoint;
                                if (countOfActions >= maxCountOfAction) countOfActions = maxCountOfAction;

                                enemyManager.currentActionPoint -= countOfActions;
                                if (enemyManager.currentActionPoint <= 0) enemyManager.currentActionPoint = 0;
                            }
                            SetActionCost();
                        }
                    }
                    else if(gamePlayManager.startPhase)
                    {
                        if (countOfActions == maxCountOfAction) canDestroy = true;

                        if (canDestroy)
                        {
                            if (player)
                            {
                                playerManager.currentActionPoint += supportActionSkill.actionValue;
                                gamePlayManager.gamePlayCanvas.DrawCardsUsingSupportCards(2, player);
                            }
                            else
                            {
                                enemyManager.currentActionPoint += supportActionSkill.actionValue;
                                gamePlayManager.gamePlayCanvas.DrawCardsUsingSupportCards(2, player);
                            }
                        }
                    }
                    break;
            }
        }
    }
    public void CheckCountOfActions(SupportActionSkillType supportActionSkillType)
    {
        if(supportActionSkillType == SupportActionSkillType.AccumulateAndDrawActionCard)
        {
            if (countOfActions < maxCountOfAction)
            {
                countOfActions++;
                SetActionCost();
            }
        }
        else
        {
            if (countOfActions > 0)
            {
                countOfActions--;
                SetActionCost();
            }
        }
    }
}
