using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GamePlayManager : MonoBehaviour
{

    [Header("State")]
    public GamePlayState currentState;
    public TurnState currentTurn;

    [Header("Phase")]
    public bool startPhase = false;
    public bool endPhase = false;
    public bool actionPhase = false;

    [Header("Attack First")]
    public bool playerAttackFirst = false;
    public bool enemyAttackFirst = false;

    [Header("EndingRound")]
    public bool playerEndingRound = false;
    public bool enemyEndingRound = false;

    [Header("Selected Character")]
    public bool playerSelectedCharacterBattleInitial = false;
    public bool enemySelectedCharacterBattleInitial = false;

    [Header("Switch Character Dying")]
    public bool playerCanSwitchCharacterDying = false;
    public bool enemyCanSwitchCharacterDying = false;

    [Header("Selected Action Card")]
    public bool playerSelectedActionCardInitial = false;
    public bool enemySelectedActionCardInitial = false;

    [Header("Character Dead First")]
    public bool playerHaveCharacterDeadFirst = false;
    public bool enemyHaveCharacterDeadFirst = false;

    [Header("Quantity")]
    public int battleCardSwitchCost = 10;
    public int quantityInitialActionCard = 5;

    [Header("Character Card List")]
    public List<CharacterCard> playerCharacterList;
    public List<CharacterCard> enemyCharacterList;

    [Header("Action Card List")]
    public List<ActionCard> playerActionCardList;
    public List<ActionCard> enemyActionCardList;

    [Header("Canvas")]
    public GamePlayCanvas gamePlayCanvas;

    public static GamePlayManager instance;
    protected UIManager uiManager => UIManager.instance;
    protected TooltipManager tooltipManager => TooltipManager.instance;
    protected NotificationManager notificationManager => NotificationManager.instance;
    protected PlayerManager playerManager => PlayerManager.instance;
    protected EnemyManager enemyManager => EnemyManager.instance;
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        gamePlayCanvas.CanvasState(false);
    }
    private void Start()
    {

        UpdateGameState(GamePlayState.SelectFirstTurn);
    }
    private void Update()
    {
        if (playerEndingRound && enemyEndingRound && !endPhase)
        {
            UpdateGameState(GamePlayState.EndPhase);
        }
    }
    //---------------------------Update State---------------------------
    public void UpdateGameState(GamePlayState gameState)
    {
        currentState = gameState;
        switch (currentState)
        {
            case GamePlayState.SelectFirstTurn:
                StartCoroutine(HandleSelectFirstTurn());
                break;

            case GamePlayState.SelectInitialActionCard:
                StartCoroutine(HandleSelectInitialActionCard());
                HideTooltip();
                break;

            case GamePlayState.SelectBattleCharacter:
                StartCoroutine(HandleSelectBattleCharacter());
                HideTooltip();
                break;

            case GamePlayState.ActionPhase:
                StartCoroutine(HandleActionPhase());
                HideTooltip();
                break;

            case GamePlayState.StartPhase:
                StartCoroutine(HandleStartPhase());
                HideTooltip();
                break;

            case GamePlayState.EndPhase:
                StartCoroutine(HandleEndPhase());
                HideTooltip();
                break;

            case GamePlayState.Victory:
                HideTooltip();
                HandleVictory();
                break;

            case GamePlayState.Lose:
                HideTooltip();
                HandleLose();
                break;

            default:
                break;
        }
    }
    public void UpdateTurnState(TurnState turnState)
    {
        currentTurn = turnState;
        switch(currentTurn)
        {
            case TurnState.YourTurn:
                if(actionPhase)
                notificationManager.SetNewNotification("Your Turn");
                break;
            case TurnState.EnemyTurn:
                if(actionPhase)
                notificationManager.SetNewNotification("Enemy Turn");
                HideHighlightsCard();
                break;
        }
    }

    //---------------------------Handle Phase---------------------------
    IEnumerator HandleSelectFirstTurn()
    {
        yield return null;
        if (uiManager != null)
        {
            uiManager.battleCanvas.CanvasState(true);
            uiManager.battleCanvas.selectTurnPanel.PanelState(true);
            uiManager.battleCanvas.skillPanel.PanelState(false);
            uiManager.battleCanvas.informationPanel.PanelState(false);
            uiManager.battleCanvas.selectInitialActionCardPanel.PanelState(false);
            uiManager.battleCanvas.switchCardBattlePanel.PanelState(false);
        }
    }
    IEnumerator HandleSelectBattleCharacter()
    {
        actionPhase = false;

        if (uiManager != null && !enemyCanSwitchCharacterDying)
        {
            uiManager.battleCanvas.selectInitialActionCardPanel.PanelState(false);
            uiManager.battleCanvas.informationPanel.PanelState(true);
            uiManager.battleCanvas.switchCardBattlePanel.PanelState(true);
            uiManager.battleCanvas.skillPanel.PanelState(false);
        }
        if (!playerSelectedCharacterBattleInitial)
        {
            gamePlayCanvas.CanvasState(true);
            yield return new WaitForSeconds(1);
            notificationManager.SetNewNotification("Select your first character");
        }
        if (playerCanSwitchCharacterDying)
        {
            uiManager.battleCanvas.switchCardBattlePanel.ActionCostState(false);
            notificationManager.SetNewNotification("Select a character to fight");
        }
        else if (enemyCanSwitchCharacterDying)
        {
            notificationManager.SetNewNotification("Enemy selecting a character to fight");
        }
    }
    IEnumerator HandleSelectInitialActionCard()
    {
        yield return null;
        if (uiManager != null)
        {
            uiManager.battleCanvas.selectInitialActionCardPanel.PanelState(true);
        }
    }
    IEnumerator HandleActionPhase()
    {
        if (actionPhase == false)
        {
            yield return new WaitForSeconds(1);
            notificationManager.SetNewNotification("Action Phase");
            actionPhase = true;
            startPhase = false;
            endPhase = false;
        }
    }
    IEnumerator HandleStartPhase()
    {
        if (startPhase == false)
        {
            startPhase = true;
            endPhase = false;
            actionPhase = false;
            playerEndingRound = false;
            enemyEndingRound = false;
            yield return new WaitForSeconds(1);
            notificationManager.SetNewNotification("Start Phase");
            yield return new WaitForSeconds(1);
            playerManager.ResetActionPoint();
            enemyManager.ResetActionPoint();
            ResetWeakness();
            yield return new WaitForSeconds(1);
            SetFirstTurn();
            yield return new WaitForSeconds(1);
            UpdateGameState(GamePlayState.ActionPhase);
        }
    }
    IEnumerator HandleEndPhase()
    {
        if (endPhase == false)
        {
            endPhase = true;
            actionPhase = false;
            startPhase = false;
            yield return new WaitForSeconds(1);
            notificationManager.SetNewNotification("End Phase");
            yield return new WaitForSeconds(1);
            StartCoroutine(gamePlayCanvas.DrawCard(2));
            yield return new WaitForSeconds(1);
            CheckBreaking();
            yield return new WaitForSeconds(1);
            ClearStatusApplying();
            yield return new WaitForSeconds(1);
            UpdateGameState(GamePlayState.StartPhase);
        }
    }
    public void CheckBreaking()
    {
        foreach (CharacterCard player in playerCharacterList)
        {
            if (player.characterStats.isApplyBreaking)
            {
                foreach(WeaknessBreaking breaking in player.characterStats.breakingList)
                {
                    StartCoroutine(player.characterStats.Bleed(breaking.weaknessType, 1));
                    break;
                }
            }
        }
        foreach (CharacterCard enemy in enemyCharacterList)
        {
            if (enemy.characterStats.isApplyBreaking)
            {
                foreach (WeaknessBreaking breaking in enemy.characterStats.breakingList)
                {
                    StartCoroutine(enemy.characterStats.Bleed(breaking.weaknessType, 1));
                    break;
                }
            }
        }
    }
    public void HandleVictory()
    {
        ClearStatusApplying();
        uiManager.battleCanvas.winLosePanel.PanelState(true);
        uiManager.battleCanvas.skillPanel.PanelState(false);
        uiManager.battleCanvas.informationPanel.PanelState(false);
        uiManager.battleCanvas.switchCardBattlePanel.PanelState(false);
    }
    public void HandleLose()
    {
        ClearStatusApplying();
        uiManager.battleCanvas.winLosePanel.PanelState(true);
        uiManager.battleCanvas.skillPanel.PanelState(false);
        uiManager.battleCanvas.informationPanel.PanelState(false);
        uiManager.battleCanvas.switchCardBattlePanel.PanelState(false);
    }
    public void ResetWeakness()
    {
        foreach (CharacterCard player in playerCharacterList)
        {
            if(player.characterStats.isApplyBreaking)
            {
                player.currentWeakness = player.characterCardData.maxWeakness;
                player.SetWeaknessText();
            }
        }
        foreach (CharacterCard enemy in enemyCharacterList)
        {
            if (enemy.characterStats.isApplyBreaking)
            {
                enemy.currentWeakness = enemy.characterCardData.maxWeakness;
                enemy.SetWeaknessText();
            }
        }
    }
    public void ClearStatusApplying()
    {
        foreach (CharacterCard player in playerCharacterList)
        {
            if (player.characterStats.isApplyingStatus)
            {
                if (player.characterStats.isSkippingRound)
                {
                    player.characterStats.ClearStatus(ActionCardActionSkillType.SkipRound);
                }
                if (player.characterStats.isDoublingDamage)
                {
                    player.characterStats.ClearStatus(ActionCardActionSkillType.DoubleDamage);
                }
                if (player.characterStats.isIncreasingAttack)
                {
                    player.characterStats.ClearStatus(ActionCardActionSkillType.IncreaseAttack);
                }
                if (player.characterStats.isSatiated)
                {
                    player.characterStats.ClearStatus(ActionCardActionSkillType.Healing);
                }
                if (player.characterStats.isShield)
                {
                    player.characterStats.ClearStatus(ActionCardActionSkillType.CreateShield);
                }
                if (player.characterStats.isReviving)
                {
                    player.characterStats.ClearStatus(ActionCardActionSkillType.Revival);
                }
            }
            if (player.characterStats.isApplyBreaking)
            {
                player.characterStats.ClearAllBreaking();
                foreach (WeaknessBreaking weaknessBreaking in player.characterStats.weaknessBreakingStateData.weaknessBreakingList)
                {
                    StartCoroutine(player.characterStats.WeaknessBreakingState(false, weaknessBreaking.weaknessType));
                }
            }
        }

        foreach (CharacterCard enemy in enemyCharacterList)
        {
            if (enemy.characterStats.isApplyingStatus)
            {
                if (enemy.characterStats.isSkippingRound)
                {
                    enemy.characterStats.ClearStatus(ActionCardActionSkillType.SkipRound);
                }
                if (enemy.characterStats.isDoublingDamage)
                {
                    enemy.characterStats.ClearStatus(ActionCardActionSkillType.DoubleDamage);
                }
                if (enemy.characterStats.isIncreasingAttack)
                {
                    enemy.characterStats.ClearStatus(ActionCardActionSkillType.IncreaseAttack);
                }
                if (enemy.characterStats.isSatiated)
                {
                    enemy.characterStats.ClearStatus(ActionCardActionSkillType.Healing);
                }
                if (enemy.characterStats.isShield)
                {
                    enemy.characterStats.ClearStatus(ActionCardActionSkillType.CreateShield);
                }
                if (enemy.characterStats.isReviving)
                {
                    enemy.characterStats.ClearStatus(ActionCardActionSkillType.Revival);
                }
            }
            if (enemy.characterStats.isApplyBreaking)
            {
                enemy.characterStats.ClearAllBreaking();
                foreach (WeaknessBreaking weaknessBreaking in enemy.characterStats.weaknessBreakingStateData.weaknessBreakingList)
                {
                    StartCoroutine(enemy.characterStats.WeaknessBreakingState(false, weaknessBreaking.weaknessType));
                }
            }
        }
        foreach (CharacterCard enemy in enemyCharacterList)
            enemy.characterCardDragHover.SelectIconState(false);
    }
    public void HighlightCardTarget(ActionTargetType actionTargetType, int actionValue, int weaknessValue, CharacterCard selfCharacterCard)
    {
        switch (actionTargetType)
        {
            case ActionTargetType.Ally:
                foreach(CharacterCard characterCard in playerCharacterList)
                {
                    if (characterCard.characterStats.isActionCharacter)
                    {
                        characterCard.characterStats.takeDamageObj.SetActive(false);
                        characterCard.characterStats.takeWeaknessObj.SetActive(false);
                        characterCard.SetTakeDamageHighlight(true);
                        characterCard.SetTakeDamageValue(actionValue);
                        foreach (Combat weaknessType in characterCard.characterStats.weaknessList)
                        {
                            if (weaknessType.combatType == selfCharacterCard.combatType)
                            {
                                characterCard.characterStats.takeDamageObj.SetActive(false);
                                characterCard.characterStats.takeWeaknessObj.SetActive(false);
                                characterCard.SetTakeWeaknessHighlight(true);
                                characterCard.SetTakeWeaknessValue(weaknessValue);
                            }
                        }
                    }
                }
                break;

            case ActionTargetType.Enemy:
                foreach(CharacterCard characterCard in enemyCharacterList)
                {
                    if (characterCard.characterStats.isActionCharacter)
                    {
                        characterCard.characterStats.takeDamageObj.SetActive(false);
                        characterCard.characterStats.takeWeaknessObj.SetActive(false);
                        characterCard.SetTakeDamageHighlight(true);
                        characterCard.SetTakeDamageValue(actionValue);
                        foreach (Combat weaknessType in characterCard.characterStats.weaknessList)
                        {
                            if (weaknessType.combatType == selfCharacterCard.combatType)
                            {
                                characterCard.SetTakeWeaknessHighlight(true);
                                characterCard.SetTakeWeaknessValue(weaknessValue);
                            }
                        }
                    }
                }
                break;

            case ActionTargetType.AllAllies:
                foreach (CharacterCard characterCard in playerCharacterList)
                {
                    characterCard.SetTakeDamageHighlight(true);
                    characterCard.SetTakeDamageValue(actionValue);
                }
                break;

            case ActionTargetType.AllEnemies:
                foreach (CharacterCard characterCard in enemyCharacterList)
                {
                    characterCard.SetTakeDamageHighlight(true);
                    characterCard.SetTakeDamageValue(actionValue);
                }
                break;
        }
    }
    public void HideHighlightsCard()
    {
        uiManager.battleCanvas.skillPanel.HideHighLight();
        foreach (CharacterCard player in playerCharacterList)
        {
            player.SetTakeDamageHighlight(false);
        }
        foreach (CharacterCard enemy in enemyCharacterList)
        {
            enemy.SetTakeDamageHighlight(false);
        }
    }
    public void HideTooltip()
    {
        uiManager.HideTooltip();
    }
    public void UnPlayCard()
    {
        uiManager.battleCanvas.playCardPanel.UnPlayCard();
    }
    public void HideSwitchCardBattle()
    {
        if (currentState == GamePlayState.ActionPhase)
        {
            uiManager.HideSwitchCardBattle();
            uiManager.ShowSkill();
            HideSelectIcon();
        }
    }
    public void HideSelectIcon()
    {
        foreach (CharacterCard player in playerCharacterList)
            player.characterCardDragHover.SelectIconState(false);

        foreach (CharacterCard enemy in enemyCharacterList)
            enemy.characterCardDragHover.SelectIconState(false);
    }
    public void DealDamageToTargets(ActionTargetType actionTargetType,  int damage, CharacterCardSkillType characterCardSkillType, CharacterCard selfCharacterCard)
    {
        switch (actionTargetType)
        {
            case ActionTargetType.Ally:
                foreach (CharacterCard characterCard in playerCharacterList)
                {
                    if (characterCard.characterStats.isActionCharacter)
                    {
                        selfCharacterCard.AttackToTarget(characterCard.transform);
                        characterCard.characterStats.TakeDamage(damage);
                        foreach (CharacterSkill characterSkill in characterCard.characterCardData.characterCard.characterSkillList)
                        {
                            if (characterSkill.characterCardSkillType == characterCardSkillType)
                            {
                                Combat weakness = selfCharacterCard.characterCardData.characterCard.combat;
                                characterCard.characterStats.AddWeakness(weakness);
                                characterCard.characterStats.TakeWeakness(weakness.combatType, characterSkill.weaknessBreakValue);
                                characterCard.SetTakeDamageValue(0);
                                characterCard.SetTakeWeaknessHighlight(false);
                            }
                        }
                    }
                }
                break;

            case ActionTargetType.Enemy:
                foreach (CharacterCard characterCard in enemyCharacterList)
                {
                    if (characterCard.characterStats.isActionCharacter)
                    {
                        selfCharacterCard.AttackToTarget(characterCard.transform);
                        characterCard.characterStats.TakeDamage(damage);
                        foreach (CharacterSkill characterSkill in characterCard.characterCardData.characterCard.characterSkillList)
                        {
                            if (characterSkill.characterCardSkillType == characterCardSkillType)
                            {
                                Combat weakness = selfCharacterCard.characterCardData.characterCard.combat;
                                characterCard.characterStats.AddWeakness(weakness);
                                characterCard.characterStats.TakeWeakness(weakness.combatType, characterSkill.weaknessBreakValue);
                                characterCard.SetTakeDamageValue(0);
                                characterCard.SetTakeWeaknessHighlight(false);
                            }
                        }
                    }
                }
                break;
        }
    }
    public IEnumerator PlayerEndRound()
    {
        playerEndingRound = true;
        notificationManager.SetNewNotification("Your is ending round");
        if (!enemyEndingRound)
        {
            playerAttackFirst = true;
            yield return new WaitForSeconds(1);
            UpdateTurnState(TurnState.EnemyTurn);
        }
    }
    public IEnumerator EnemyEndRound()
    {
        enemyEndingRound = true;
        notificationManager.SetNewNotification("Enemy is ending round");
        if (!playerEndingRound)
        {
            enemyAttackFirst = true;
            yield return new WaitForSeconds(1);
            UpdateTurnState(TurnState.YourTurn);
        }
    }
    public void SetFirstTurn()
    {
        if (playerAttackFirst)
        {
            playerAttackFirst = false;
            notificationManager.SetNewNotification("Your attack first");
            UpdateTurnState(TurnState.YourTurn);
        }
        else if (enemyAttackFirst)
        {
            enemyAttackFirst = false;
            notificationManager.SetNewNotification("Enemy attack first");
            UpdateTurnState(TurnState.EnemyTurn);
        }
    }
}
