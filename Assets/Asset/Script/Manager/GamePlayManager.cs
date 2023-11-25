using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    [Header("State")]
    public GamePlayState currentState;
    public TurnState currentTurn;

    [Header("Phase")]
    public bool startPhase = false;
    public bool endPhase = false;
    public bool actionPhase = false;

    [Header("Win Lose State")]
    public bool playerWin = false;
    public bool enemyWin = false;

    [Header("Attack First")]
    public bool playerAttackFirst = false;
    public bool enemyAttackFirst = false;

    [Header("EndingRound")]
    public bool playerEndingRound = false;
    public bool enemyEndingRound = false;

    [Header("Action Attack")]
    public bool playerAttacking = false;
    public bool enemyAttacking = false;

    [Header("Using Support Card")]
    public bool playerActionSupportCard = false;
    public bool enemyActionSupportCard = false;

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

    [Header("Drawing Action Cards")]
    public bool playerDrawingActionCard = false;
    public bool enemyDrawingActionCard = false;

    [Header("Quantity")]
    public int battleCardSwitchCost = 10;
    public int quantityInitialActionCard = 5;

    [Header("Character Card List")]
    public List<CharacterCard> playerCharacterList;
    public List<CharacterCard> enemyCharacterList;

    [Header("Action Card List")]
    public List<ActionCard> playerActionCardList;
    public List<ActionCard> enemyActionCardList;

    [Header("Support Card List")]
    public List<SupportCard> playerSupportCardList;
    public List<SupportCard> enemySupportCardList;

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
        if (instance == null)
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
        if (playerWin || enemyWin) return;

        currentState = gameState;
        switch (currentState)
        {
            case GamePlayState.SelectFirstTurn:
                HandleSelectFirstTurn();
                break;

            case GamePlayState.SelectInitialActionCard:
                HandleSelectInitialActionCard();
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
        if (playerWin || enemyWin) return;

        currentTurn = turnState;
        switch (currentTurn)
        {
            case TurnState.YourTurn:
                if (actionPhase)
                {
                    notificationManager.SetNewNotification("Your Turn");
                    notificationManager.notificationText.transform.DOScale(1.5f, 0.25f).SetLoops(2,LoopType.Yoyo);
                    AudioManager.instance.PlayYourTurn();
                }
                break;
            case TurnState.EnemyTurn:
                if (actionPhase)
                {
                    notificationManager.SetNewNotification("Enemy Turn");
                    notificationManager.notificationText.transform.DOScale(1.5f, 0.25f).SetLoops(2, LoopType.Yoyo);
                    AudioManager.instance.PlayEnemyTurn();
                }
                HideHighlightsCard();
                break;
        }
    }

    //---------------------------Handle Phase---------------------------
    void HandleSelectFirstTurn()
    {
        playerWin = false;
        enemyWin = false;

        uiManager.battleCanvas.CanvasState(true);
        uiManager.battleCanvas.selectTurnPanel.PanelState(true);
        uiManager.battleCanvas.skillPanel.PanelState(false);
        uiManager.battleCanvas.informationPanel.PanelState(false);
        uiManager.battleCanvas.selectInitialActionCardPanel.PanelState(false);
        uiManager.battleCanvas.switchCardBattlePanel.PanelState(false);
    }
    void HandleSelectInitialActionCard()
    {
        uiManager.battleCanvas.selectInitialActionCardPanel.PanelState(true);
        if (uiManager.tutorialCanvas != null)
        {
            uiManager.tutorialCanvas.ActionTutorial(TutorialType.SelectActionCardInitial);
        }
    }
    IEnumerator HandleSelectBattleCharacter()
    {
        if (!enemyCanSwitchCharacterDying)
        {
            uiManager.tutorialCanvas.ActionTutorial(TutorialType.uiTutorial);

            uiManager.battleCanvas.settingPanel.PanelState(true);
            uiManager.battleCanvas.informationPanel.PanelState(true);
            uiManager.battleCanvas.switchCardBattlePanel.PanelState(true);
            uiManager.battleCanvas.selectInitialActionCardPanel.PanelState(false);
            uiManager.battleCanvas.skillPanel.PanelState(false);
        }
        else
        {
            notificationManager.SetNewNotification("Enemy selecting a character to fight");
        }

        if (!playerSelectedCharacterBattleInitial)
        {
            gamePlayCanvas.CanvasState(true);
            uiManager.battleCanvas.switchCardBattlePanel.SetSwitchCardBattleText("Hãy chọn một nhân vật để xuất chiến");
            uiManager.battleCanvas.switchCardBattlePanel.ActionCostState(false);
            yield return new WaitForSeconds(1);
            notificationManager.SetNewNotification("Select your first character");
        }
        if (playerCanSwitchCharacterDying)
        {
            uiManager.battleCanvas.switchCardBattlePanel.ActionCostState(false);
            notificationManager.SetNewNotification("Select a character to fight");
        }

    }

    IEnumerator HandleActionPhase()
    {
        if (actionPhase == false)
        {
            yield return new WaitForSeconds(1);
            AudioManager.instance.PlayPhase();
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
            uiManager.battleCanvas.informationPanel.PlayerEndingRoundObjState(playerEndingRound);
            uiManager.battleCanvas.informationPanel.EnemyEndingRoundObjState(enemyEndingRound);
            yield return new WaitForSeconds(1);
            notificationManager.SetNewNotification("Start Phase");
            AudioManager.instance.PlayPhase();
            yield return new WaitForSeconds(1);
            playerManager.ResetActionPoint();
            enemyManager.ResetActionPoint();
            ResetWeakness();
            yield return new WaitForSeconds(1);
            yield return StartCoroutine(PlayerActionSupportCard(ActionStartPhase.StartRound));
            yield return StartCoroutine(EnemyActionSupportCard(ActionStartPhase.StartRound));
            yield return StartCoroutine(PlayerActionSupportCard(ActionStartPhase.StartActionPhaseAndEndCheckPhase));
            yield return StartCoroutine(EnemyActionSupportCard(ActionStartPhase.StartActionPhaseAndEndCheckPhase));
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
            AudioManager.instance.PlayPhase();
            yield return new WaitForSeconds(1);

            //while (CheckCharacterPlayerDead()) yield return null;

            yield return StartCoroutine(PlayerActionSupportCard(ActionStartPhase.EndRound));
            yield return StartCoroutine(EnemyActionSupportCard(ActionStartPhase.EndRound));
            yield return StartCoroutine(PlayerActionSupportCard(ActionStartPhase.StartActionPhaseAndEndCheckPhase));
            yield return StartCoroutine(EnemyActionSupportCard(ActionStartPhase.StartActionPhaseAndEndCheckPhase));

            while (playerDrawingActionCard || enemyDrawingActionCard) yield return null;

            yield return new WaitForSeconds(1);
            StartCoroutine(ActionBreaking());
            ClearStatusApplying();

            while (playerCanSwitchCharacterDying || enemyCanSwitchCharacterDying) yield return null;

            yield return new WaitForSeconds(1);
            StartCoroutine(gamePlayCanvas.DrawCardEndPhase(2));
            yield return new WaitForSeconds(2);
            UpdateGameState(GamePlayState.StartPhase);
        }
    }
    public bool CheckCharacterPlayerDead()
    {
        bool dead = true;
        foreach (CharacterCard player in playerCharacterList)
        {
            if (player.characterStats.isActionCharacter)
            {
                dead = false;
            }
        }
        if (dead) return true;
        else return false;
    }
    public IEnumerator ActionBreaking()
    {
        foreach (CharacterCard player in playerCharacterList)
        {
            if (player.characterStats.isApplyBreaking)
            {
                foreach (WeaknessBreaking breaking in player.characterStats.breakingList)
                {
                    if (!player.characterStats.isFreezing && !player.characterStats.isDetention)
                    {
                        StartCoroutine(player.characterStats.Bleed(breaking.weaknessType, 3));
                    }
                }
            }
        }
        foreach (CharacterCard enemy in enemyCharacterList)
        {
            if (enemy.characterStats.isApplyBreaking)
            {
                foreach (WeaknessBreaking breaking in enemy.characterStats.breakingList)
                {
                    if (!enemy.characterStats.isFreezing && !enemy.characterStats.isDetention)
                    {
                        StartCoroutine(enemy.characterStats.Bleed(breaking.weaknessType, 3));
                    }

                }
            }
        }
        yield return null;
    }
    public void HandleVictory()
    {
        HandleEndGame();
        playerWin = true;

    }
    public void HandleLose()
    {
        HandleEndGame();
        enemyWin = true;
    }
    public void HandleEndGame()
    {
        ClearStatusApplying();
        uiManager.battleCanvas.winLosePanel.PanelState(true);
        uiManager.battleCanvas.skillPanel.PanelState(false);
        uiManager.battleCanvas.informationPanel.PanelState(false);
        uiManager.battleCanvas.switchCardBattlePanel.PanelState(false);
        uiManager.battleCanvas.settingPanel.PanelState(false);

        uiManager.tutorialCanvas.ResetIsUsed(false);

        actionPhase = false;
        endPhase = false;
        startPhase = false;
    }
    public void ResetWeakness()
    {
        foreach (CharacterCard player in playerCharacterList)
        {
            if (player.currentWeakness == 0 && !player.characterStats.isDead)
            {
                player.currentWeakness = player.characterCardData.maxWeakness;
                player.SetWeaknessText();
            }
        }
        foreach (CharacterCard enemy in enemyCharacterList)
        {
            if (enemy.currentWeakness == 0 && !enemy.characterStats.isDead)
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
            player.characterStats.ClearAllStatus();
            if (player.characterStats.isApplyBreaking)
            {
                player.characterStats.ClearAllBreaking();
            }
            foreach (WeaknessBreaking weaknessBreaking in player.characterStats.weaknessBreakingStateData.weaknessBreakingList)
            {
                StartCoroutine(player.characterStats.WeaknessBreakingState(false, weaknessBreaking.weaknessType));
            }
        }

        foreach (CharacterCard enemy in enemyCharacterList)
        {
            enemy.characterStats.ClearAllStatus();

            if (enemy.characterStats.isApplyBreaking)
            {
                enemy.characterStats.ClearAllBreaking();
            }
            foreach (WeaknessBreaking weaknessBreaking in enemy.characterStats.weaknessBreakingStateData.weaknessBreakingList)
            {
                StartCoroutine(enemy.characterStats.WeaknessBreakingState(false, weaknessBreaking.weaknessType));
            }
        }
    }
    public void HighlightCardTarget(ActionTargetType actionTargetType, int actionValue, int weaknessValue, CharacterCard selfCharacterCard)
    {
        switch (actionTargetType)
        {
            case ActionTargetType.Ally:
                foreach (CharacterCard characterCard in playerCharacterList)
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
                foreach (CharacterCard characterCard in enemyCharacterList)
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
    public void ChangeSwitchCardBattleState()
    {
        if (playerCanSwitchCharacterDying)
        {
            uiManager.battleCanvas.switchCardBattlePanel.PanelState(true);
            uiManager.battleCanvas.skillPanel.PanelState(false);
        }
        else
        {
            if (currentState == GamePlayState.ActionPhase)
            {
                uiManager.battleCanvas.switchCardBattlePanel.PanelState(false);
                uiManager.battleCanvas.skillPanel.PanelState(true);
                HideSelectIcon();
            }
        }
    }
    public void HideSelectIcon()
    {
        foreach (CharacterCard player in playerCharacterList)
            player.characterCardDragHover.SelectIconState(false);

    }
    public void DealDamageToTargets(ActionTargetType actionTargetType, int damage, CharacterCardSkillType characterCardSkillType, CharacterCard selfCharacterCard)
    {
        switch (actionTargetType)
        {
            case ActionTargetType.Ally:
                foreach (CharacterCard targetCharacter in playerCharacterList)
                {
                    if (targetCharacter.characterStats.isActionCharacter)
                    {
                        StartCoroutine(ActionDealDamage(targetCharacter, damage, characterCardSkillType, selfCharacterCard));
                    }
                }
                break;

            case ActionTargetType.Enemy:
                foreach (CharacterCard targetCharacter in enemyCharacterList)
                {
                    if (targetCharacter.characterStats.isActionCharacter)
                    {
                        StartCoroutine(ActionDealDamage(targetCharacter, damage, characterCardSkillType, selfCharacterCard));
                    }
                }
                break;
        }
    }
    public IEnumerator ActionDealDamage(CharacterCard targetCharacter, int damage, CharacterCardSkillType characterCardSkillType, CharacterCard selfCharacterCard)
    {
        if (currentTurn == TurnState.YourTurn) playerAttacking = true;
        else if (currentTurn == TurnState.EnemyTurn) enemyAttacking = true;
        if (characterCardSkillType == CharacterCardSkillType.ElementalBurst)
        {
            selfCharacterCard.PlaySound(SoundType.UseElementalBurst);
            yield return new WaitForSeconds(selfCharacterCard.SoundLength());
        }
        AudioManager.instance.PlayNAAttack();
        selfCharacterCard.AttackToTarget(targetCharacter.transform);
        StartCoroutine(TakeDamageTarget(targetCharacter, damage, characterCardSkillType, selfCharacterCard));
    }
    public IEnumerator TakeDamageTarget(CharacterCard targetCharacter, int damage, CharacterCardSkillType characterCardSkillType, CharacterCard selfCharacterCard)
    {
        yield return new WaitForSeconds(targetCharacter.moveDuration);
        targetCharacter.characterStats.TakeDamage(damage);
        foreach (CharacterSkill characterSkill in selfCharacterCard.characterCardData.characterCard.characterSkillList)
        {
            if (characterSkill.characterCardSkillType == characterCardSkillType)
            {
                targetCharacter.SetTakeDamageValue(0);
                targetCharacter.SetTakeWeaknessHighlight(false);
                Combat weakness = selfCharacterCard.characterCardData.characterCard.combat;
                targetCharacter.characterStats.AddWeakness(weakness);
                targetCharacter.characterStats.TakeWeakness(weakness.combatType, characterSkill.weaknessBreakValue);
            }
        }
    }
    public IEnumerator PlayerEndRound()
    {
        playerEndingRound = true;
        uiManager.battleCanvas.informationPanel.PlayerEndingRoundObjState(playerEndingRound);
        notificationManager.SetNewNotification("Your is ending round");
        yield return new WaitForSeconds(1.5f);
        if (!enemyEndingRound)
        {
            playerAttackFirst = true;
            UpdateTurnState(TurnState.EnemyTurn);
        }
    }
    public IEnumerator EnemyEndRound()
    {
        enemyEndingRound = true;
        uiManager.battleCanvas.informationPanel.EnemyEndingRoundObjState(enemyEndingRound);
        notificationManager.SetNewNotification("Enemy is ending round");
        yield return new WaitForSeconds(1.5f);
        if (!playerEndingRound)
        {
            enemyAttackFirst = true;
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
    public SupportCard GetSupportCardUsing(SupportActionSkillType supportActionSkillType)
    {
        foreach (SupportCard supportCard in playerSupportCardList)
        {
            SupportActionSkillType supportActionType = supportCard.supportCardData.actionCard.supportActionSkill.supportSkillType;
            if (supportActionType == supportActionSkillType)
            {
                return supportCard;
            }
        }
        return null;
    }
    public void UseSupportCard(ActionStartPhase actionStartPhase, bool isPlayer)
    {
        StartCoroutine(PlayerActionSupportCard(actionStartPhase));
    }
    public IEnumerator PlayerActionSupportCard(ActionStartPhase startPhase)
    {
        foreach (SupportCard supportCard in playerSupportCardList)
        {
            SupportActionSkill supportActionSkill = supportCard.supportCardData.actionCard.supportActionSkill;
            ActionStartPhase actionStartPhase = supportActionSkill.actionStartPhase;
            ActionTargetType actionTargetType = supportActionSkill.actionTargetType;
            List<CharacterCard> targetList = PlayerDetermineTarget(actionTargetType);

            if (actionStartPhase == startPhase)
            {
                playerActionSupportCard = true;
                supportCard.DoSupportAction(supportActionSkill, targetList, true);
                yield return new WaitForSeconds(1);
            }
        }
        for (int i = playerSupportCardList.Count - 1; i >= 0; i--)
        {
            if (playerSupportCardList[i].countOfActions == playerSupportCardList[i].maxCountOfAction &&
                playerSupportCardList[i].canDestroy)
            {
                playerSupportCardList[i].canvasGroup.DOFade(0, 0.5f);
                AudioManager.instance.PlayDestroySupportCard();
                yield return new WaitForSeconds(0.5f);
                Destroy(playerSupportCardList[i].gameObject);
                playerSupportCardList.RemoveAt(i);
            }
        }
        playerActionSupportCard = false;
    }
    public IEnumerator EnemyActionSupportCard(ActionStartPhase startPhase)
    {
        foreach (SupportCard supportCard in enemySupportCardList)
        {
            SupportActionSkill supportActionSkill = supportCard.supportCardData.actionCard.supportActionSkill;
            ActionStartPhase actionStartPhase = supportActionSkill.actionStartPhase;
            ActionTargetType actionTargetType = supportActionSkill.actionTargetType;
            List<CharacterCard> targetList = EnemyDetermineTarget(actionTargetType);
            if (actionStartPhase == startPhase)
            {
                supportCard.DoSupportAction(supportActionSkill, targetList, false);
                yield return new WaitForSeconds(1);
            }
        }
        for (int i = enemySupportCardList.Count - 1; i >= 0; i--)
        {
            if (enemySupportCardList[i].countOfActions == enemySupportCardList[i].maxCountOfAction &&
                enemySupportCardList[i].canDestroy)
            {
                enemySupportCardList[i].canvasGroup.DOFade(0, 0.5f);
                AudioManager.instance.PlayDestroySupportCard();
                yield return new WaitForSeconds(0.5f);
                Destroy(enemySupportCardList[i].gameObject);
                enemySupportCardList.RemoveAt(i);
            }
        }
    }
    public List<CharacterCard> PlayerDetermineTarget(ActionTargetType actionTargetType)
    {
        List<CharacterCard> targetList = new List<CharacterCard>();
        switch (actionTargetType)
        {
            case ActionTargetType.Ally:
                foreach (CharacterCard characterCard in playerCharacterList)
                {
                    if (characterCard.characterStats.isActionCharacter && !characterCard.characterStats.isDead)
                        targetList.Add(characterCard);
                }
                break;

            case ActionTargetType.Enemy:
                foreach (CharacterCard characterCard in enemyCharacterList)
                {
                    if (characterCard.characterStats.isActionCharacter && !characterCard.characterStats.isDead)
                        targetList.Add(characterCard);
                }
                break;

            case ActionTargetType.AllAllies:
                foreach (CharacterCard characterCard in playerCharacterList)
                {
                    if (!characterCard.characterStats.isDead)
                        targetList.Add(characterCard);
                }
                break;

            case ActionTargetType.AllEnemies:
                foreach (CharacterCard characterCard in enemyCharacterList)
                {
                    if (!characterCard.characterStats.isDead)
                        targetList.Add(characterCard);
                }
                break;
            case ActionTargetType.DeadFirstAlly:
                foreach (CharacterCard characterCard in playerCharacterList)
                {
                    if (characterCard.characterStats.isDeadFirst)
                    {
                        targetList.Add(characterCard);
                    }
                }
                break;
            case ActionTargetType.AllyLowestHealth:
                CharacterCard target = playerCharacterList[0];
                for (int i = 0; i < playerCharacterList.Count; i++)
                {
                    if (playerCharacterList[i].currentHealth < target.currentHealth)
                    {
                        target = playerCharacterList[i];
                    }
                }
                targetList.Add(target);
                break;
        }
        return targetList;
    }
    public List<CharacterCard> EnemyDetermineTarget(ActionTargetType actionTargetType)
    {
        List<CharacterCard> targetList = new List<CharacterCard>();
        switch (actionTargetType)
        {
            case ActionTargetType.Ally:
                foreach (CharacterCard characterCard in enemyCharacterList)
                {
                    if (characterCard.characterStats.isActionCharacter && !characterCard.characterStats.isDead)
                        targetList.Add(characterCard);
                }
                break;

            case ActionTargetType.Enemy:
                foreach (CharacterCard characterCard in playerCharacterList)
                {
                    if (characterCard.characterStats.isActionCharacter && !characterCard.characterStats.isDead)
                        targetList.Add(characterCard);
                }
                break;

            case ActionTargetType.AllAllies:
                foreach (CharacterCard characterCard in enemyCharacterList)
                {
                    if (!characterCard.characterStats.isDead)
                        targetList.Add(characterCard);
                }
                break;

            case ActionTargetType.AllEnemies:
                foreach (CharacterCard characterCard in playerCharacterList)
                {
                    if (!characterCard.characterStats.isDead)
                        targetList.Add(characterCard);
                }
                break;
            case ActionTargetType.DeadFirstAlly:
                foreach (CharacterCard characterCard in enemyCharacterList)
                {
                    if (characterCard.characterStats.isDeadFirst)
                    {
                        targetList.Add(characterCard);
                    }
                }
                break;
            case ActionTargetType.AllyLowestHealth:
                CharacterCard target = enemyCharacterList[0];
                for (int i = 0; i < enemyCharacterList.Count; i++)
                {
                    if (enemyCharacterList[i].currentHealth < target.currentHealth)
                    {
                        target = enemyCharacterList[i];
                    }
                }
                targetList.Add(target);
                break;
        }
        return targetList;
    }
}