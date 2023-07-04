using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public bool canSummon;
    public bool summoned;

    public int state = 0;
    public int id;
    public int mana;
    public int numberCardsInDeck;

    public string cardName;
    public string cardDescription; //will show when click

    public Animator animator;

    public Sprite cardSprite;
    public Image cardImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI manaText;

    public CardData card;
    public GameObject playerField;
    public Draggable draggable;
    public TurnSystem turnSystem;
    private void Start()
    {
        turnSystem = FindObjectOfType<TurnSystem>();
        draggable = GetComponent<Draggable>();  
        animator = GetComponent<Animator>();
        playerField = GameObject.Find("PlayerField");

        CardsToHand();
        id = card.id;
        cardName = card.cardName;
        mana = card.mana;
        cardDescription = card.description;
        nameText.text = "" + cardName;
        manaText.text = "" + mana;
        LoadImage();
    }
    private void Update()
    {
        Summon();
        SetAnimation();
    }
    private void LoadImage()
    {
        cardSprite = card.cardSprite;
        cardImage.sprite = cardSprite;
    }
    void SetAnimation()
    {
        switch (state)
        {
            case 0: break;
            case 1: animator.SetInteger("State", 1); break;
            case 2: animator.SetInteger("State", 2); break;
        }
    }
    void CardsToHand()
    {
        PlayerDeck playerDeck = FindObjectOfType<PlayerDeck>();
        if (playerDeck.cardDataList.Count != 0)
        {
            numberCardsInDeck = playerDeck.deckSize-1;
            card = playerDeck.cardDataList[numberCardsInDeck];
            playerDeck.cardDataList.RemoveAt(numberCardsInDeck);
        }
    }
    void Summon()
    {
        if (turnSystem.currentMana >= mana && !summoned)
        {
            canSummon = true;
        }
        else
        {
            canSummon = false;
        }
        if (canSummon)
        {
            draggable.enabled = true; 
            if(this.transform.parent == playerField.transform)
            {
                SummonAction();
            }
        }
        else
        {
            draggable.enabled= false;
        }
    }
    void SummonAction()
    {
        summoned = true;
        turnSystem.currentMana -= mana;
    }

}