using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CardListBattle : MonoBehaviour
{
    public ContentSizeFitter contentSizeFitter;

    public CardSelectType thisCardType;

    public CollectionManager collectionManager => CollectionManager.instance;
    protected DeckManager deckManager => DeckManager.instance;
    public void Start()
    {
        contentSizeFitter = GetComponent<ContentSizeFitter>();
    }
    private void Update()
    {
        CheckSize();
    }
    public void CheckSize()
    {
        if (contentSizeFitter != null)
        {
            if (transform.childCount > 7)
            {
                contentSizeFitter.enabled = true;
            }
            else
            {
                contentSizeFitter.enabled = false;
            }
        }

        if (thisCardType == CardSelectType.CharacterCard)
        {
            DropZone dropZone = GetComponent<DropZone>();
            if (collectionManager.characterCardCount >= deckManager.characterCardMaxSize)
            {
                dropZone.enabled = false;
            }
            else
            {
                if (!dropZone.enabled)
                {
                    dropZone.enabled = true;
                }
            }
        }
        if(thisCardType == CardSelectType.ActionCard)
        {
            DropZone dropZone = GetComponent<DropZone>();
            if (collectionManager.actionCardCount >= deckManager.actionCardMaxSize)
            {
                dropZone.enabled = false;
            }
            else
            {
                if (!dropZone.enabled)
                {
                    dropZone.enabled = true;
                }
            }
        }
    }
}
