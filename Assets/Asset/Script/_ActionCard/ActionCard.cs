using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class ActionCard : CardBase
{
    [Header("Data")]
    public int quantityInDeck;
    public int quantityMaxInDeck;
    public int actionCost;
    [TextArea] public string description;
    public ActionCardData actionCardData;

    [Header("Image")]
    public Sprite cardSprite;
    public Image cardImage;
    public Image backImage;
    public Image cardBack;

    [Header("Text")]
    public TextMeshProUGUI actionCostText;
    public TextMeshProUGUI quantityText;
    public TextMeshProUGUI quantityInDeckText;
    public TextMeshProUGUI quantitySelectedText;


    [Header("GameObject")]
    [SerializeField] private GameObject quantityInDeckObj;
    [SerializeField] private GameObject quantitySelectedObj;
    [SerializeField] private GameObject quantityObj;
    [SerializeField] private GameObject manaObj;

    [Header("Component")]
    public ActionCardDragHover actionCardDragHover;

    private void Start()
    {
        actionCardDragHover = GetComponent<ActionCardDragHover>();
    }
    public void GetCardData(ActionCardData actionCardData)
    {
        this.actionCardData = actionCardData;
        quantityMaxInDeck = actionCardData.quantityMaxInDeck;
        description = actionCardData.cardDescription;

        SetQuantity(actionCardData.quantityMaxInDeck);
        SetCardImage(actionCardData.cardSprite);
        SetBackImage(actionCardData.actionCard.colorRarity);
        SetActionCost(actionCardData.actionCost);
    }
    public void SetActionCost(int value)
    {
        actionCost = value;
        actionCostText.text = actionCost.ToString();
    }
    public void SetCardImage(Sprite sprite)
    {
        cardSprite = sprite;
        cardImage.sprite = cardSprite;
    }
    public void SetBackImage(Color color)
    {
        backImage.color = color;
    }
    public void SetQuantity(int value)
    {
        quantityMaxInDeck = value;

        if(quantityText != null)
        quantityText.text = quantityMaxInDeck.ToString();
    }
    public void ManaState(bool state)
    {
        manaObj.gameObject.SetActive(state);
    }
    public void QuantityInDeckState(bool state)
    {
        quantityInDeckObj.SetActive(state);
    }
    public void QuantitySelectedState(bool state)
    {
        quantitySelectedObj.SetActive(state);
    }
    public void QuantityState(bool state)
    {
        quantityObj.SetActive(state);
    }
    public void CardBackState(bool state)
    {
        cardBack.gameObject.SetActive(state);
    }
    public void CardState(bool state)
    {
        gameObject.SetActive(state);
    }
    public void RecallCard(int quantity)
    {
        quantityInDeck -= quantity;
        if(quantityInDeck < 0) quantityInDeck = 0;

        quantitySelectedText.text = "Selected " + quantityInDeck.ToString() + " Card";
        quantityInDeckText.text = quantityInDeck.ToString();
        collectionManager.actionCardDataList.Remove(actionCardData);
    }
    public void AddCard(int quantity)
    {
        quantityInDeck += quantity;
        if(quantityInDeck > quantityMaxInDeck) quantityInDeck = quantityMaxInDeck;

        quantitySelectedText.text = "Selected " + quantityInDeck.ToString() + " Card";
        quantityInDeckText.text = quantityInDeck.ToString();
        collectionManager.actionCardDataList.Add(actionCardData);
    }
    public void PlayCard()
    {
        if (actionCardData.isSupportCard)
        {
            gamePlayManager.gamePlayCanvas.SpawnSupportCard(this, true);
        }
        else
        {
            for (int i = 0; i < actionCardData.actionCard.actionSkillList.Count; i++)
            {
                ActionCardSkill actionCardSkill = actionCardData.actionCard.actionSkillList[i];
                ActionTargetType actionTargetType = actionCardSkill.actionTargetType;
                List<CharacterCard> targetList = gamePlayManager.PlayerDetermineTarget(actionTargetType);
                DoAction(actionCardSkill, targetList, actionCardSkill.statusList, true);
            }
        }
    }
    public void DoAction(ActionCardSkill actionCardSkill, List<CharacterCard> targetList, List<Status> statusList, bool isPlayer)
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
                reduceSkillActionPointsAction.DoAction(actionCardSkill,targetList, statusList);
                break;
            case ActionCardActionSkillType.DoubleDamage:
                DoubleDamageAction doubleDamageAction = new DoubleDamageAction();
                doubleDamageAction.DoAction(actionCardSkill,targetList,statusList);
                break;
            case ActionCardActionSkillType.SkipRound:
                SkipRoundAction skipRoundAction = new SkipRoundAction();
                skipRoundAction.DoAction(actionCardSkill,targetList,statusList);
                break;
            case ActionCardActionSkillType.Revival:
                RevivalAction revivalAction = new RevivalAction();
                revivalAction.DoAction(actionCardSkill,targetList,statusList); 
                break;

                //Need check bool
            case ActionCardActionSkillType.IncreaseActionPoint:
                if (isPlayer) playerManager.RecoveryActionPoint(actionCardSkill.actionValue);
                else enemyManager.RecoveryActionPoint(actionCardSkill.actionValue);
                break;

            case ActionCardActionSkillType.SkillPointRecovery:
                if (isPlayer) playerManager.RecoverySkillPoint(actionCardSkill.actionValue);
                else enemyManager.RecoverySkillPoint(actionCardSkill.actionValue);
                break;

            case ActionCardActionSkillType.GetRandomActionPoints:
                int random = Random.Range(1, 3);
                if (random == 1)
                {
                    if (isPlayer) playerManager.RecoveryActionPoint(actionCardSkill.actionValue);
                    else enemyManager.RecoveryActionPoint(actionCardSkill.actionValue);
                }
                break;
        }
    }
    public void CheckActionCard()
    {
        actionCardDragHover.canPlayCard = true;

        if(actionCardData.isSupportCard)
        {
            if(gamePlayManager.gamePlayCanvas.playerSupportCardField.transform.childCount >= 4)
            {
                actionCardDragHover.canPlayCard = false;
                actionCardDragHover.ReturnCard();
                notificationManager.SetNewNotification("There are already 4 support cards on the table, no more can be used");
            }
        }
        else
        {
            for (int i = 0; i < actionCardData.actionCard.actionSkillList.Count; i++)
            {
                ActionCardSkill actionCardSkill = actionCardData.actionCard.actionSkillList[i];
                ActionTargetType actionTargetType = actionCardSkill.actionTargetType;
                List<CharacterCard> targetList = gamePlayManager.PlayerDetermineTarget(actionTargetType);
                bool canReturnCard = true;
                switch (actionCardSkill.actionSkillType)
                {
                    case ActionCardActionSkillType.Healing:
                        foreach (CharacterCard characterCard in targetList)
                        {
                            if (characterCard.currentHealth < characterCard.characterCardData.maxHealth && !characterCard.characterStats.isSatiated)
                            {
                                canReturnCard = false;
                            }
                        }
                        if (canReturnCard)
                        {
                            actionCardDragHover.ReturnCard();
                            notificationManager.SetNewNotification("Card cannot be used at this time");
                        }
                        break;
                    case ActionCardActionSkillType.ReduceSkillActionPoints:
                        canReturnCard = false;
                        foreach (CharacterCard characterCard in targetList)
                        {
                            if (characterCard.characterStats.isReducingSkillActionPoints)
                            {
                                canReturnCard = true;
                            }
                        }
                        if (canReturnCard)
                        {
                            actionCardDragHover.ReturnCard();
                            notificationManager.SetNewNotification("Card cannot be used at this time");
                        }
                        break;
                    case ActionCardActionSkillType.IncreaseAttack:
                        foreach (CharacterCard characterCard in targetList)
                        {
                            if (characterCard.characterStats.isIncreasingAttack)
                            {
                                actionCardDragHover.ReturnCard();
                                notificationManager.SetNewNotification("Card cannot be used at this time");
                            }
                        }
                        break;
                    case ActionCardActionSkillType.Revival:
                        if (targetList.Count == 0)
                        {
                            actionCardDragHover.ReturnCard();
                            notificationManager.SetNewNotification("Card cannot be used at this time");
                        }
                        break;
                    case ActionCardActionSkillType.DoubleDamage:
                        foreach (CharacterCard characterCard in targetList)
                        {
                            if (characterCard.characterStats.isDoublingDamage)
                            {
                                actionCardDragHover.ReturnCard();
                                notificationManager.SetNewNotification("Card cannot be used at this time");
                            }
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
                            actionCardDragHover.ReturnCard();
                            notificationManager.SetNewNotification("Card cannot be used at this time");
                        }
                        break;
                }
            }
        }
    }
}
