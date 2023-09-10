using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionState
{
    Attack,
    SwitchCharacter,
    UseActionCard,
    EndRound,
}
public class EnemyAIController : ControllerBase
{
    public bool actioned = false;
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
            if (!gamePlayManager.enemySelectedCharacterBattleInitial && 
                gamePlayManager.playerSelectedCharacterBattleInitial && 
                !gamePlayManager.actionPhase)
            {
                yield return new WaitForSeconds(1);
                yield return StartCoroutine(InitialBattleCharacterCard());
            }
            else if (currentCharacterCard != null && currentCharacterCard.characterStats.isDead)
            {
                yield return new WaitForSeconds(1);
                yield return StartCoroutine(Action(ActionState.SwitchCharacter));
            }
            else if (!actioned && gamePlayManager.actionPhase && !gamePlayManager.enemyEndingRound &&
                gamePlayManager.currentTurn == TurnState.EnemyTurn && 
                gamePlayManager.currentState != GamePlayState.SelectBattleCharacter)
            {
                actioned = true;
                yield return new WaitForSeconds(1);
                //yield return StartCoroutine(Action(ActionState.UseActionCard));
                yield return new WaitForSeconds(Random.Range(2,3));
                yield return StartCoroutine(Action(ActionState.Attack));
                actioned = false;
            }
        }
    }

    private IEnumerator InitialBattleCharacterCard()
    {
        notificationManager.SetNewNotification("Enemy is selecting character");
        yield return new WaitForSeconds(Random.Range(2, 3));
        gamePlayManager.enemyCharacterList[0].characterCardDragHover.HandleCardSelecting();
        gamePlayManager.enemySelectedCharacterBattleInitial = true;
        currentCharacterCard = gamePlayManager.enemyCharacterList[0];
        gamePlayManager.SetFirstTurn();
        yield return new WaitForSeconds(1);
        gamePlayManager.UpdateGameState(GamePlayState.ActionPhase);
    }

    private IEnumerator Action(ActionState actionState)
    {
        yield return new WaitForSeconds(1);
        currentActionState = actionState;
        switch (currentActionState)
        {
            case ActionState.Attack:
                HandleAttackState();
                break;

            case ActionState.SwitchCharacter:
                StartCoroutine(HandleSwitchCharacter());
                break;

            case ActionState.UseActionCard:
                StartCoroutine(HandleUseActionCard());
                break;

            case ActionState.EndRound:
                break;

            default:
                break;
        }
    }
    public void HandleAttackState()
    {
        if (gamePlayManager.currentTurn == TurnState.YourTurn) return;

        foreach (CharacterCard characterCard in gamePlayManager.enemyCharacterList)
        {
            if (characterCard.characterStats.isActionCharacter && !characterCard.characterStats.isDead)
            {
                SkillSelection(characterCard);
            }
        }
    }
    public IEnumerator HandleSwitchCharacter()
    {
        yield return null;
        foreach(CharacterCard characterCard in gamePlayManager.enemyCharacterList)
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
        yield return null;
        foreach(ActionCard actionCard in gamePlayManager.enemyActionCardList)
        {
            CheckTarget(actionCard);

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
            EnemyPlayCard(actionCard);
        }
    }
    public void EnemyPlayCard(ActionCard actionCard)
    {
        List<CharacterCard> playerCharacterList = gamePlayManager.playerCharacterList;
        List<CharacterCard> enemyCharacterList = gamePlayManager.enemyCharacterList;
        for (int i = 0; i < actionCard.actionCardData.actionCard.actionSkillList.Count; i++)
        {
            Debug.Log("enemy play card");
            ActionCardSkill actionCardSkill = actionCard.actionCardData.actionCard.actionSkillList[i];
            ActionTargetType actionTargetType = actionCardSkill.actionTargetType;
            List<CharacterCard> targetList = EnemyDetermineTarget(actionTargetType, playerCharacterList, enemyCharacterList);
            EnemyDoAction(actionCard.actionCardData, actionCardSkill, targetList, actionCardSkill.statusList);
        }
        uiManager.battleCanvas.playCardPanel.PanelState(true);
        uiManager.battleCanvas.playCardPanel.GetCardInfo(actionCard, actionCard.actionCardDragHover);
        StartCoroutine(uiManager.battleCanvas.playCardPanel.ShowCardInfo());
        actionCard.CardState(false);
        gamePlayManager.enemyActionCardList.Remove(actionCard);
        enemyManager.ConsumeActionPoint(actionCard.actionCost);
    }
    public List<CharacterCard> EnemyDetermineTarget(ActionTargetType actionTargetType, List<CharacterCard> playerCharacterList, List<CharacterCard> enemyCharacterList)
    {
        List<CharacterCard> targetList = new List<CharacterCard>();
        switch (actionTargetType)
        {
            case ActionTargetType.Ally:
                foreach (CharacterCard characterCard in enemyCharacterList)
                {
                    if (characterCard.characterStats.isActionCharacter)
                        targetList.Add(characterCard);
                }
                break;

            case ActionTargetType.Enemy:
                foreach (CharacterCard characterCard in playerCharacterList)
                {
                    if (characterCard.characterStats.isActionCharacter)
                        targetList.Add(characterCard);
                }
                break;

            case ActionTargetType.AllAllies:
                foreach (CharacterCard characterCard in enemyCharacterList)
                {
                    targetList.Add(characterCard);
                }
                break;

            case ActionTargetType.AllEnemies:
                foreach (CharacterCard characterCard in playerCharacterList)
                {
                    targetList.Add(characterCard);
                }
                break;
            case ActionTargetType.DeadFirstAlly:
                foreach (CharacterCard characterCard in enemyCharacterList)
                {
                    if (characterCard.characterStats.isDead && characterCard.characterStats.isDeadFirst)
                    {
                        targetList.Add(characterCard);
                    }
                }
                break;
        }
        return targetList;
    }
    public void EnemyDoAction(ActionCardData actionCardData, ActionCardSkill actionCardSkill, List<CharacterCard> targetList, List<Status> statusList)
    {
        switch (actionCardSkill.actionSkillType)
        {
            case ActionCardActionSkillType.Healing:
                HealingAction healingAction = new HealingAction();
                healingAction.DoAction(actionCardData, actionCardSkill, targetList, statusList);
                break;
            case ActionCardActionSkillType.IncreaseAttack:
                IncreaseAttackAction increaseAttackAction = new IncreaseAttackAction();
                increaseAttackAction.DoAction(actionCardData, actionCardSkill, targetList, statusList);
                break;
            case ActionCardActionSkillType.SkillPointRecovery:
                enemyManager.RecoverySkillPoint(actionCardSkill.actionValue);
                break;
            case ActionCardActionSkillType.IncreaseBurstPoint:
                IncreaseBurstPointAction increaseBurstPointAction = new IncreaseBurstPointAction();
                increaseBurstPointAction.DoAction(actionCardData, actionCardSkill, targetList, statusList);
                break;
            case ActionCardActionSkillType.CreateShield:
                CreateShiedAction createShiedAction = new CreateShiedAction();
                createShiedAction.DoAction(actionCardData, actionCardSkill, targetList, statusList);
                break;
            case ActionCardActionSkillType.ReduceSkillActionPoints:
                ReduceSkillActionPointsAction reduceSkillActionPointsAction = new ReduceSkillActionPointsAction();
                reduceSkillActionPointsAction.DoAction(actionCardData, actionCardSkill, targetList, statusList);
                break;
            case ActionCardActionSkillType.DoubleDamage:
                DoubleDamageAction doubleDamageAction = new DoubleDamageAction();
                doubleDamageAction.DoAction(actionCardData, actionCardSkill, targetList, statusList);
                break;
            case ActionCardActionSkillType.SkipRound:
                SkipRoundAction skipRoundAction = new SkipRoundAction();
                skipRoundAction.DoAction(actionCardData, actionCardSkill, targetList, statusList);
                break;
            case ActionCardActionSkillType.Revival:
                RevivalAction revivalAction = new RevivalAction();
                revivalAction.DoAction(actionCardData, actionCardSkill, targetList, statusList);
                break;
        }
    }
    public void CheckTarget(ActionCard actionCard)
    {
        actionCard.actionCardDragHover.canPlayCard = true;
        List<CharacterCard> playerCharacterList = gamePlayManager.playerCharacterList;
        List<CharacterCard> enemyCharacterList = gamePlayManager.enemyCharacterList;
        for (int i = 0; i < actionCard.actionCardData.actionCard.actionSkillList.Count; i++)
        {
            ActionCardSkill actionCardSkill = actionCard.actionCardData.actionCard.actionSkillList[i];
            ActionTargetType actionTargetType = actionCardSkill.actionTargetType;
            List<CharacterCard> targetList = EnemyDetermineTarget(actionTargetType, playerCharacterList, enemyCharacterList);
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
            }
        }
    }
        public void SkillSelection(CharacterCard characterCard)
    {
        if(gamePlayManager.currentState != GamePlayState.SelectBattleCharacter)
        {
            foreach (CharacterSkill characterSkill in characterCard.characterCardData.characterCard.characterSkillList)
            {
                if (characterCard.currentBurstPoint >= characterCard.characterCardData.burstPointMax)
                {
                    if (characterSkill.characterCardSkillType == CharacterCardSkillType.ElementalBurst)
                    {
                        PerformSkill(characterCard, characterSkill);
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
                            PerformSkill(characterCard, characterSkill);
                            return;
                        }

                    }
                    else if (index == 2)
                    {
                        if (characterSkill.characterCardSkillType == CharacterCardSkillType.ElementalSkill)
                        {
                            PerformSkill(characterCard, characterSkill);
                            return;

                        }
                    }
                }
            }
        }
    }
    public void PerformSkill(CharacterCard characterCard, CharacterSkill characterSkill)
    {
        if (enemyManager.currentActionPoint >= characterSkill.actionPointCost)
        {
            foreach (Skill skill in characterSkill.actionSkillList)
            {
                UseSkill(characterCard, characterSkill, skill.actionTargetType, skill.actionValue);
            }
        }
        else
        {
            StartCoroutine(gamePlayManager.EnemyEndRound());
        }
    }
    public void UseSkill(CharacterCard characterCard, CharacterSkill characterSkill, ActionTargetType actionTargetType, int actionValue)
    {
        enemyManager.ConsumeSkillPoint(characterSkill.skillPointCost);
        characterCard.SetBurstPoint(characterSkill.burstPointCost);
        enemyManager.ConsumeActionPoint(characterSkill.actionPointCost);
        if(actionTargetType == ActionTargetType.Enemy)
        {
            gamePlayManager.DealDamageToTargets(ActionTargetType.Ally, actionValue);
        }
        if (gamePlayManager.currentTurn == TurnState.EnemyTurn)
        {
            if (!gamePlayManager.playerEndingRound)
                gamePlayManager.UpdateTurnState(TurnState.YourTurn);
            else
                notificationManager.SetNewNotification("Enemy turn continues...");
        }

        if (characterCard.characterStats.isReducingSkillActionPoints)
        {
            characterCard.characterStats.ClearStatus(ActionCardActionSkillType.ReduceSkillActionPoints);
        }
        if (characterCard.characterStats.isDoublingDamage)
        {
            characterCard.characterStats.ClearStatus(ActionCardActionSkillType.DoubleDamage);
        }
        if (characterCard.characterStats.isIncreasingAttack)
        {
            characterCard.characterStats.ClearStatus(ActionCardActionSkillType.IncreaseAttack);
        }
    }
}
