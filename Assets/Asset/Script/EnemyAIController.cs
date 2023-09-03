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
    public float minRandom = 2f;
    public float maxRandom = 3f;
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
                yield return StartCoroutine(InitialBattleCharacterCard());
            }
            if(!actioned && gamePlayManager.actionPhase && !gamePlayManager.enemyEndingRound &&
                gamePlayManager.currentTurn == TurnState.EnemyTurn)
            {
                actioned = true;
                yield return StartCoroutine(Action(ActionState.Attack));
                actioned = false;
            }
            if(currentCharacterCard != null && currentCharacterCard.characterStats.isDead)
            {
                yield return StartCoroutine(Action(ActionState.SwitchCharacter));
            }
        }
    }

    private IEnumerator InitialBattleCharacterCard()
    {
        notificationManager.SetNewNotification("Enemy is selecting character");
        yield return new WaitForSeconds(Random.Range(minRandom, maxRandom));
        gamePlayManager.enemyCharacterList[0].characterCardDragHover.HandleCardSelecting();
        gamePlayManager.enemySelectedCharacterBattleInitial = true;
        currentCharacterCard = gamePlayManager.enemyCharacterList[0];
        gamePlayManager.SetFirstTurn();
        yield return new WaitForSeconds(1);
        gamePlayManager.UpdateGameState(GamePlayState.ActionPhase);
    }

    private IEnumerator Action(ActionState actionState)
    {
        yield return new WaitForSeconds(Random.Range(minRandom, maxRandom));
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
                break;

            case ActionState.EndRound:
                break;

            default:
                break;
        }
    }
    public void HandleAttackState()
    {
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
            if (characterCard.characterStats.isDead)
            {
                characterCard.characterCardDragHover.HandleCardSelected();
            }
            else
            {
                yield return new WaitForSeconds(1);
                currentCharacterCard = characterCard;
                characterCard.characterCardDragHover.HandleCardSelecting();
                break;
            }
        }
    }
    public void HandleUseActionCardState()
    {
        foreach(ActionCard actionCard in gamePlayManager.enemyActionCardList)
        {
            if(actionCard.actionCardData.actionCost >= enemyManager.currentActionPoint)
            {
                UseActionCard();
            }
        }
    }


    public void HandleEndRound()
    {

    }
    private void UseActionCard()
    {
    }
    public void SkillSelection(CharacterCard characterCard)
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
                    if(characterSkill.characterCardSkillType == CharacterCardSkillType.NormalAttack)
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
        characterCard.SetBurstPoint(characterSkill.burstPointCost);
        enemyManager.ConsumeActionPoint(characterSkill.actionPointCost);
        enemyManager.ConsumeSkillPoint(characterSkill.skillPointCost);
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
    }
}
