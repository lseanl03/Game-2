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

    public ActionCardData card;
    public GameObject playerField;
    public Draggable draggable;
    public TurnManager turnSystem;
    private void Start()
    {
        turnSystem = FindObjectOfType<TurnManager>();
        draggable = GetComponent<Draggable>();  
        animator = GetComponent<Animator>();
        playerField = GameObject.Find("PlayerField");

        CardsToHand();
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
    }
    private void LoadImage()
    {
        cardSprite = card.cardSprite;
        cardImage.sprite = cardSprite;
    }
    void CardsToHand()
    {
        //PlayerDeck playerDeck = FindObjectOfType<PlayerDeck>();
        //if (playerDeck.cardDataList.Count != 0)
        //{
        //    numberCardsInDeck = playerDeck.deckSize-1;
        //    card = playerDeck.cardDataList[numberCardsInDeck];
        //    playerDeck.cardDataList.RemoveAt(numberCardsInDeck);
        //}
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