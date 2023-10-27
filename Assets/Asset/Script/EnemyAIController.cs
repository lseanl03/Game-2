using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public enum ActionState
{
    Attack,
    UseActionCard,
}
public class EnemyAIController : ControllerBase
{
    public bool isUsingCard = false;
    public ActionState currentActionState;
    public CharacterCard currentCharacterCard;

    private void Start()
    {
        StartCoroutine(ManageAI());
    }
    private IEnumerator ManageAI()
    {
        while (true)
        {
            yield return null;
            if (CanSelectInitialCard())
            {
                yield return StartCoroutine(HandleSelectInitialBattleCharacter());
            }
            else if (CanSwitchCard())
            {
                yield return StartCoroutine(HandleSwitchCharacterDying());
            }
            else if (CC())
            {
                yield return StartCoroutine(HandleCCState());
            }
            else if (CanAction())
            {
                if (!gamePlayManager.enemyAttacking)
                {
                    yield return StartCoroutine(Action(ActionState.UseActionCard));
                    yield return StartCoroutine(Action(ActionState.Attack));
                }
            }
        }
    }

    private IEnumerator HandleCCState()
    {
        yield return new WaitForSeconds(Random.Range(2, 3));

        while (gamePlayManager.enemyWin || gamePlayManager.playerWin) yield return null;

        if (enemyManager.currentActionPoint >= gamePlayManager.battleCardSwitchCost)
        {
            HandleSwitchCharacterCC();
        }
        else
        {
            HandleEndRound();
        }
        yield return null;
    }

    bool CanSelectInitialCard()
    {
        return !gamePlayManager.enemySelectedCharacterBattleInitial && 
            gamePlayManager.playerSelectedCharacterBattleInitial;
    }
    bool CanSwitchCard()
    {
        if (currentCharacterCard == null) return false;
        return currentCharacterCard.characterStats.isDead;
    }
    bool CanAction()
    {
        if (currentCharacterCard == null) return false;
        return gamePlayManager.actionPhase && !gamePlayManager.enemyEndingRound &&
                gamePlayManager.currentTurn == TurnState.EnemyTurn &&
                gamePlayManager.currentState != GamePlayState.SelectBattleCharacter &&
                !currentCharacterCard.characterStats.isFreezing && !currentCharacterCard.characterStats.isDetention;
    }
    bool CC()
    {
        if (currentCharacterCard == null || gamePlayManager.currentTurn == TurnState.YourTurn || gamePlayManager.enemyEndingRound) return false;
        return currentCharacterCard.characterStats.isFreezing || currentCharacterCard.characterStats.isDetention;

    }
    private IEnumerator Action(ActionState actionState)
    {
        yield return new WaitForSeconds(Random.Range(2,3));
        currentActionState = actionState;
        switch (currentActionState)
        {
            case ActionState.Attack:
                StartCoroutine(HandleAttackState());
                break;

            case ActionState.UseActionCard:
                StartCoroutine(HandleUseActionCard());
                break;

        }
    }
    private IEnumerator HandleSelectInitialBattleCharacter()
    {
        notificationManager.SetNewNotification("Enemy is selecting character");
        yield return new WaitForSeconds(Random.Range(2, 3));

        while (gamePlayManager.enemyWin || gamePlayManager.playerWin) yield return null;

        gamePlayManager.enemyCharacterList[0].characterCardDragHover.HandleCardSelecting();
        gamePlayManager.enemySelectedCharacterBattleInitial = true;
        currentCharacterCard = gamePlayManager.enemyCharacterList[0];
        gamePlayManager.SetFirstTurn();
        gamePlayManager.gamePlayCanvas.playerCharacterCardField.GetComponent<HorizontalLayoutGroup>().enabled = false;
        gamePlayManager.gamePlayCanvas.enemyCharacterCardField.GetComponent<HorizontalLayoutGroup>().enabled = false;
        gamePlayManager.UpdateGameState(GamePlayState.ActionPhase);

        uiManager.tutorialCanvas.ActionTutorial(TutorialType.ActionCardTutorial);

    }
    private void HandleEndRound()
    {
        StartCoroutine(gamePlayManager.EnemyEndRound());
    }
    public IEnumerator HandleAttackState()
    {
        while(isUsingCard || gamePlayManager.currentTurn == TurnState.YourTurn || 
            gamePlayManager.enemyEndingRound || !gamePlayManager.actionPhase) yield return null;
        Debug.Log("Enemy Attack");

        foreach (CharacterCard characterCard in gamePlayManager.enemyCharacterList)
        {
            if (characterCard.characterStats.isActionCharacter && !characterCard.characterStats.isDead)
            {
                EnemySkillSelection();
            }
        }
    }
    public void HandleSwitchCharacterCC()
    {
        foreach (CharacterCard characterCard in gamePlayManager.enemyCharacterList)
        {
            if (!characterCard.characterStats.isDead && 
                !characterCard.characterStats.isFreezing && !characterCard.characterStats.isDetention)
            {
                currentCharacterCard.characterCardDragHover.HandleCardSelected();
                currentCharacterCard = characterCard;
                characterCard.characterCardDragHover.HandleCardSelecting();
                enemyManager.ConsumeActionPoint(gamePlayManager.battleCardSwitchCost);
                break;
            }
        }
        if (gamePlayManager.currentTurn == TurnState.EnemyTurn)
        {
            if (!gamePlayManager.playerEndingRound)
                gamePlayManager.UpdateTurnState(TurnState.YourTurn);
            else
                notificationManager.SetNewNotification("Enemy turn continues...");
        }
        if (gamePlayManager.currentState == GamePlayState.SelectBattleCharacter)
        {
            gamePlayManager.UpdateGameState(GamePlayState.ActionPhase);
        }
    }
    public IEnumerator HandleSwitchCharacterDying()
    {
        yield return new WaitForSeconds(Random.Range(3f,5f));
        while (gamePlayManager.enemyWin || gamePlayManager.playerWin) yield return null;

        foreach (CharacterCard characterCard in gamePlayManager.enemyCharacterList)
        {
            if (!characterCard.characterStats.isDead)
            {
                currentCharacterCard = characterCard;
                characterCard.characterCardDragHover.HandleCardSelecting();
                break;
            }
        }
        if(gamePlayManager.currentState == GamePlayState.SelectBattleCharacter)
        {
            gamePlayManager.UpdateGameState(GamePlayState.ActionPhase);
        }
    }
    private IEnumerator HandleUseActionCard()
    {
        while (gamePlayManager.enemyWin || gamePlayManager.playerWin) yield return null;

        Debug.Log("Enemy Use Card");

        if (gamePlayManager.currentTurn == TurnState.YourTurn || !gamePlayManager.actionPhase) yield return null;

        foreach (ActionCard actionCard in gamePlayManager.enemyActionCardList)
        {
            EnemyCheckActionCard(actionCard);

            if (actionCard.actionCardDragHover.canPlayCard)
            {
                ActionCardAction(actionCard);
            }
            break;
        }
    }
    public void ActionCardAction(ActionCard actionCard)
    {
        if(enemyManager.currentActionPoint > actionCard.actionCost)
        {
            StartCoroutine(EnemyPlayCard(actionCard));
        }
    }
    public IEnumerator EnemyPlayCard(ActionCard actionCard)
    {
        isUsingCard = true;
        uiManager.battleCanvas.playCardPanel.PanelState(true);
        uiManager.battleCanvas.playCardPanel.GetCardInfo(actionCard);
        StartCoroutine(uiManager.battleCanvas.playCardPanel.ShowCardInfo());
        yield return new WaitForSeconds(1.5f);
        Destroy(actionCard.gameObject);

        if (actionCard.actionCardData.isSupportCard)
        {
            gamePlayManager.gamePlayCanvas.SpawnSupportCard(actionCard, false);
        }
        else
        {
            for (int i = 0; i < actionCard.actionCardData.actionCard.actionSkillList.Count; i++)
            {
                ActionCardSkill actionCardSkill = actionCard.actionCardData.actionCard.actionSkillList[i];
                ActionTargetType actionTargetType = actionCardSkill.actionTargetType;
                List<CharacterCard> targetList = gamePlayManager.EnemyDetermineTarget(actionTargetType);
                actionCard.DoAction(actionCardSkill, targetList,actionCardSkill.statusList,false);
            }
        }

        gamePlayManager.enemyActionCardList.Remove(actionCard);
        enemyManager.ConsumeActionPoint(actionCard.actionCost);
        isUsingCard = false;
    }
    public void EnemyCheckActionCard(ActionCard actionCard)
    {
        actionCard.actionCardDragHover.canPlayCard = true;
        if (actionCard.actionCardData.isSupportCard)
        {
            if (gamePlayManager.gamePlayCanvas.enemySupportCardField.transform.childCount >= 4)
            {
                actionCard.actionCardDragHover.canPlayCard = false;
            }
        }
        else
        {
            for (int i = 0; i < actionCard.actionCardData.actionCard.actionSkillList.Count; i++)
            {
                ActionCardSkill actionCardSkill = actionCard.actionCardData.actionCard.actionSkillList[i];
                ActionTargetType actionTargetType = actionCardSkill.actionTargetType;
                List<CharacterCard> targetList = gamePlayManager.EnemyDetermineTarget(actionTargetType);
                bool canReturnCard = true;
                switch (actionCardSkill.actionSkillType)
                {
                    case ActionCardActionSkillType.Healing:
                        foreach (CharacterCard characterCard in targetList)
                        {
                            if (characterCard.currentHealth < characterCard.characterCardData.maxHealth &&
                                !characterCard.characterStats.isSatiated && !characterCard.characterStats.isDead)
                            {
                                canReturnCard = false;
                            }
                        }
                        if (canReturnCard)
                        {
                            actionCard.actionCardDragHover.canPlayCard = false;
                        }
                        break;
                    case ActionCardActionSkillType.ReduceSkillActionPoints:
                        canReturnCard = false;
                        foreach (CharacterCard characterCard in targetList)
                        {
                            if (characterCard.characterStats.isReducingSkillActionPoints && !characterCard.characterStats.isDead)
                            {
                                canReturnCard = true;
                            }
                        }
                        if (canReturnCard)
                        {
                            actionCard.actionCardDragHover.canPlayCard = false;
                        }
                        break;
                    case ActionCardActionSkillType.IncreaseAttack:
                        foreach (CharacterCard characterCard in targetList)
                        {
                            if (characterCard.characterStats.isIncreasingAttack)
                            {
                                actionCard.actionCardDragHover.canPlayCard = false;
                            }
                        }
                        break;
                    case ActionCardActionSkillType.Revival:
                        if (targetList.Count == 0)
                        {
                            actionCard.actionCardDragHover.canPlayCard = false;
                        }
                        break;
                    case ActionCardActionSkillType.IncreaseBurstPoint:
                        foreach (CharacterCard characterCard in targetList)
                        {
                            if (characterCard.currentBurstPoint < characterCard.characterCardData.burstPointMax)
                            {
                                canReturnCard = false;
                            }
                        }
                        if (canReturnCard)
                        {
                            actionCard.actionCardDragHover.canPlayCard = false;
                        }
                        break;
                    case ActionCardActionSkillType.DoubleDamage:
                        foreach (CharacterCard characterCard in targetList)
                        {
                            if (characterCard.characterStats.isDoublingDamage)
                            {
                                actionCard.actionCardDragHover.canPlayCard = false;
                            }
                        }
                        break;
                }
            }
        }
    }
        public void EnemySkillSelection()
    {
        if(gamePlayManager.currentState != GamePlayState.SelectBattleCharacter)
        {
            foreach (CharacterSkill characterSkill in currentCharacterCard.characterCardData.characterCard.characterSkillList)
            {
                if (currentCharacterCard.currentBurstPoint >= currentCharacterCard.characterCardData.burstPointMax)
                {
                    if (characterSkill.characterCardSkillType == CharacterCardSkillType.ElementalBurst)
                    {
                        EnemyPerformSkill(characterSkill, currentCharacterCard.currentEBActionPointCost, currentCharacterCard.currentEBActionValue);
                        return;
                    }
                }
                else
                {
                    int index = Random.Range(1, 3);
                    if (index == 1)
                    {
                        if (characterSkill.characterCardSkillType == CharacterCardSkillType.NormalAttack)
                        {
                            EnemyPerformSkill(characterSkill, currentCharacterCard.currentNAActionPointCost, currentCharacterCard.currentNAActionValue);
                            return;
                        }

                    }
                    else if (index == 2)
                    {
                        if (characterSkill.characterCardSkillType == CharacterCardSkillType.ElementalSkill)
                        {
                            EnemyPerformSkill(characterSkill, currentCharacterCard.currentESActionPointCost, currentCharacterCard.currentESActionValue);
                            return;
                        }
                    }
                }
            }
        }
    }
    public void EnemyPerformSkill(CharacterSkill characterSkill, int actionCost, int actionValue)
    {
        if (enemyManager.currentActionPoint >= actionCost) StartCoroutine(EnemyUseSkill(characterSkill, actionCost, actionValue));
        else HandleEndRound();
    }
    public IEnumerator EnemyUseSkill(CharacterSkill characterSkill, int actionCost, int actionValue)
    {
        foreach (Skill skill in characterSkill.skillList)
        {
            if (skill.actionTargetType == ActionTargetType.Enemy)
            {
                gamePlayManager.DealDamageToTargets(ActionTargetType.Ally, actionValue, characterSkill.characterCardSkillType, currentCharacterCard);
            }
        }
        while (gamePlayManager.enemyAttacking) yield return null;

        enemyManager.ConsumeSkillPoint(characterSkill.skillPointCost);
        enemyManager.ConsumeActionPoint(actionCost);
        currentCharacterCard.SetBurstPoint(characterSkill.burstPointCost);
        currentCharacterCard.characterStats.ClearStatusAfterAttack();
        if (gamePlayManager.currentTurn == TurnState.EnemyTurn)
        {
            if (!gamePlayManager.playerEndingRound)
            {
                gamePlayManager.UpdateTurnState(TurnState.YourTurn);
                if (!uiManager.tutorialCanvas.isShowedWeaknessTutorial)
                {
                    uiManager.tutorialCanvas.ActionTutorial(TutorialType.WeaknessTutorial);
                }
            }
            else
                notificationManager.SetNewNotification("Enemy turn continues...");
        }
    }
}
