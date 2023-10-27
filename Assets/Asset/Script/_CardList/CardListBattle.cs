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
    protected PlayerManager playerManager => PlayerManager.instance;
    public void Start()
    {
        contentSizeFitter = GetComponent<ContentSizeFitter>();
    }
    private void Update()
    {
        CheckSize();
    }
    public void OnEnable()
    {
    }
    public void CheckSize()
    {
        if (contentSizeFitter != null && transform.childCount>0)
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
            CardDropZone dropZone = GetComponent<CardDropZone>();
            if (collectionManager.characterCardCount >= playerManager.characterCardMaxSize)
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
            CardDropZone dropZone = GetComponent<CardDropZone>();
            if (collectionManager.actionCardCount >= playerManager.actionCardMaxSize)
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
