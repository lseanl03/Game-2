using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyAIController : ControllerBase
{
    public float minRandom = 2f;
    public float maxRandom = 3f;
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
            if (gamePlayManager.playerSelectedCharacterBattleInitial && !gamePlayManager.actionPhase)
            {
                yield return StartCoroutine(InitialBattleCharacterCard());
            }

            if (gamePlayManager.actionPhase && gamePlayManager.currentTurn == TurnState.EnemyTurn)
            {
                yield return StartCoroutine(Action());
            }
        }
    }

    private IEnumerator InitialBattleCharacterCard()
    {
        notificationManager.SetNewNotification("Enemy is selecting character");
        yield return new WaitForSeconds(Random.Range(minRandom, maxRandom));
        gamePlayManager.enemyCharacterList[0].characterCardDragHover.HandleCardSelecting();
        gamePlayManager.enemySelectedCharacterBattleInitial = true;
        gamePlayManager.UpdateGameState(GamePlayState.ActionPhase);
    }

    private IEnumerator Action()
    {
        yield return new WaitForSeconds(Random.Range(minRandom, maxRandom));
        foreach (CharacterCard characterCard in gamePlayManager.enemyCharacterList)
        {
            if (characterCard.characterStats.isActionCharacter)
            {
                SkillSelection(characterCard);
            }
        }
    }
    public void SkillSelection(CharacterCard characterCard)
    {
        foreach (CharacterSkill characterSkill in characterCard.characterCardData.characterCard.characterSkillList)
        {
            if (characterCard.currentBurstPoint >= characterCard.burstPointMax)
            {
                if (characterSkill.characterCardSkillType == CharacterCardSkillType.ElementalBurst)
                {
                        Debug.Log("aa");
                    if (enemyManager.currentActionPoint >= characterSkill.actionPointCost)
                    {
                        Debug.Log("a");
                        foreach (Skill skill in characterSkill.actionSkillList)
                        {
                            UseSkill(characterCard, characterSkill, skill.actionTargetType, skill.actionValue);
                            return;
                        }
                        return;
                    }
                    else
                    {
                        Debug.Log("b");
                    }
                }
            }
            else
            {
                int index = Random.Range(1, 3);
                Debug.Log(index);
                if (index == 1)
                {
                    if(characterSkill.characterCardSkillType == CharacterCardSkillType.NormalAttack)
                    {
                        Debug.Log("aa");

                        if (enemyManager.currentActionPoint >= characterSkill.actionPointCost)
                        {
                            foreach(Skill skill in characterSkill.actionSkillList)
                            {
                                UseSkill(characterCard, characterSkill, skill.actionTargetType, skill.actionValue);
                                return;
                            }
                        }
                        else
                        {
                            Debug.Log("b");
                        }
                    }

                }
                else if (index == 2)
                {
                    if (characterSkill.characterCardSkillType == CharacterCardSkillType.ElementalSkill)
                    {
                        if (enemyManager.currentActionPoint >= characterSkill.actionPointCost)
                        {
                            foreach (Skill skill in characterSkill.actionSkillList)
                            {
                                UseSkill(characterCard, characterSkill, skill.actionTargetType, skill.actionValue);
                                return;
                            }
                        }
                        else
                        {
                            Debug.Log("b");
                        }
                    }
                }
            }
        }
    }

    public void UseSkill(CharacterCard characterCard, CharacterSkill characterSkill, ActionTargetType actionTargetType, int actionValue)
    {
        Debug.Log("UseSkill");
        characterCard.BurstPointConsumption(characterSkill.burstPointCost);
        enemyManager.ConsumeActionPoint(characterSkill.actionPointCost);
        enemyManager.ConsumeSkillPoint(characterSkill.skillPointCost);
        if(actionTargetType == ActionTargetType.Enemy)
        {
            gamePlayManager.DealDamageToTargets(ActionTargetType.Ally, actionValue);
        }
        gamePlayManager.UpdateTurnState(TurnState.YourTurn);
    }
    private IEnumerator EndRound()
    {
        yield return new WaitForSeconds(Random.Range(minRandom, maxRandom));
        gamePlayManager.EnemyEndRound();
    }
}
