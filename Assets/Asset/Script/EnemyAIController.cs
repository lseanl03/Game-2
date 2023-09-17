using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
                yield return StartCoroutine(Action(ActionState.UseActionCard));
                yield return new WaitForSeconds(1);
                yield return StartCoroutine(Action(ActionState.Attack));
                actioned = false;
            }
        }
    }

    private IEnumerator InitialBattleCharacterCard()
    {
        Debug.Log("InitialBattleCharacterCard");
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
        Debug.Log("HandleAttackState");

        if (gamePlayManager.currentTurn == TurnState.YourTurn) return;

        foreach (CharacterCard characterCard in gamePlayManager.enemyCharacterList)
        {
            if (characterCard.characterStats.isActionCharacter && !characterCard.characterStats.isDead)
            {
                SkillSelection();
            }
        }
    }
    public IEnumerator HandleSwitchCharacter()
    {
        yield return null;
        Debug.Log("HandleSwitchCharacter");

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
        yield return null;
        Debug.Log("HandleUseActionCard");

        if (gamePlayManager.currentTurn == TurnState.YourTurn) yield return null;

        foreach (ActionCard actionCard in gamePlayManager.enemyActionCardList)
        {
            CheckTarget(actionCard);

            if (actionCard.actionCardDragHover.canPlayCard)
            {
                ActionCardAction(actionCard);
            }
            break;
        }
        yield return null;
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
            EnemyDoAction(actionCardSkill, targetList, actionCardSkill.statusList);
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
    public void EnemyDoAction(ActionCardSkill actionCardSkill, List<CharacterCard> targetList, List<Status> statusList)
    {
        switch (actionCardSkill.actionSkillType)
        {
            case ActionCardActionSkillType.Healing:
                HealingAction healingAction = new HealingAction();
                healingAction.DoAction(actionCardSkill, targetList, statusList);
                break;
            case ActionCardActionSkillType.IncreaseAttack:
                IncreaseAttackAction increaseAttackAction = new IncreaseAttackAction();
                increaseAttackAction.DoAction(actionCardSkill, targetList, statusList);
                break;
            case ActionCardActionSkillType.SkillPointRecovery:
                enemyManager.RecoverySkillPoint(actionCardSkill.actionValue);
                break;
            case ActionCardActionSkillType.IncreaseBurstPoint:
                IncreaseBurstPointAction increaseBurstPointAction = new IncreaseBurstPointAction();
                increaseBurstPointAction.DoAction(actionCardSkill, targetList, statusList);
                break;
            case ActionCardActionSkillType.CreateShield:
                CreateShiedAction createShiedAction = new CreateShiedAction();
                createShiedAction.DoAction(actionCardSkill, targetList, statusList);
                break;
            case ActionCardActionSkillType.ReduceSkillActionPoints:
                ReduceSkillActionPointsAction reduceSkillActionPointsAction = new ReduceSkillActionPointsAction();
                reduceSkillActionPointsAction.DoAction(actionCardSkill, targetList, statusList);
                break;
            case ActionCardActionSkillType.DoubleDamage:
                DoubleDamageAction doubleDamageAction = new DoubleDamageAction();
                doubleDamageAction.DoAction(actionCardSkill, targetList, statusList);
                break;
            case ActionCardActionSkillType.SkipRound:
                SkipRoundAction skipRoundAction = new SkipRoundAction();
                skipRoundAction.DoAction(actionCardSkill, targetList, statusList);
                break;
            case ActionCardActionSkillType.Revival:
                RevivalAction revivalAction = new RevivalAction();
                revivalAction.DoAction(actionCardSkill, targetList, statusList);
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
        public void SkillSelection()
    {
        if(gamePlayManager.currentState != GamePlayState.SelectBattleCharacter)
        {
            foreach (CharacterSkill characterSkill in currentCharacterCard.characterCardData.characterCard.characterSkillList)
            {
                if (currentCharacterCard.currentBurstPoint >= currentCharacterCard.characterCardData.burstPointMax)
                {
                    if (characterSkill.characterCardSkillType == CharacterCardSkillType.ElementalBurst)
                    {
                        PerformSkill(characterSkill);
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
                            PerformSkill(characterSkill);
                            return;
                        }

                    }
                    else if (index == 2)
                    {
                        if (characterSkill.characterCardSkillType == CharacterCardSkillType.ElementalSkill)
                        {
                            PerformSkill(characterSkill);
                            return;

                        }
                    }
                }
            }
        }
    }
    public void PerformSkill( CharacterSkill characterSkill)
    {
        if (enemyManager.currentActionPoint >= characterSkill.actionPointCost)
        {
            StartCoroutine(UseSkill(characterSkill));
        }
        else
        {
            StartCoroutine(gamePlayManager.EnemyEndRound());
        }
    }
    public IEnumerator UseSkill(CharacterSkill characterSkill)
    {
        foreach (Skill skill in characterSkill.actionSkillList)
        {
            enemyManager.ConsumeSkillPoint(characterSkill.skillPointCost);
            enemyManager.ConsumeActionPoint(characterSkill.actionPointCost);
            currentCharacterCard.SetBurstPoint(characterSkill.burstPointCost);
            if (skill.actionTargetType == ActionTargetType.Enemy)
            {
                gamePlayManager.DealDamageToTargets(ActionTargetType.Ally, skill.actionValue, characterSkill.characterCardSkillType, currentCharacterCard);
            }
        }
        yield return new WaitForSeconds(1);
        if (gamePlayManager.currentTurn == TurnState.EnemyTurn)
        {
            if (!gamePlayManager.playerEndingRound)
                gamePlayManager.UpdateTurnState(TurnState.YourTurn);
            else
                notificationManager.SetNewNotification("Enemy turn continues...");
        }

        if (currentCharacterCard.characterStats.isReducingSkillActionPoints)
        {
            currentCharacterCard.characterStats.ClearStatus(ActionCardActionSkillType.ReduceSkillActionPoints);
        }
        if (currentCharacterCard.characterStats.isDoublingDamage)
        {
            currentCharacterCard.characterStats.ClearStatus(ActionCardActionSkillType.DoubleDamage);
        }
        if (currentCharacterCard.characterStats.isIncreasingAttack)
        {
            currentCharacterCard.characterStats.ClearStatus(ActionCardActionSkillType.IncreaseAttack);
        }
    }
}
