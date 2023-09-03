using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectCardCanvas : CanvasBase
{
    public Button characterCardButton;
    public Button actionCardButton;
    public Button startGameButton;
    public TextMeshProUGUI quantityCharacterCardText;
    public TextMeshProUGUI quantityActionCardText;
    private void Start()
    {
        CharacterCardButtonState(true);
        ActionCardButtonState(false);
    }
    public void OpenCharacterCardPanel()
    {
        CharacterCardButtonState(true);
        ActionCardButtonState(false);
        collectionManager.collectionCanvas.ChangeContent(CardSelectType.CharacterCard);
    }
    public void OpenActionCardPanel()
    {
        ActionCardButtonState(true);
        CharacterCardButtonState(false);
        collectionManager.collectionCanvas.ChangeContent(CardSelectType.ActionCard);
    }
    public void QuantityCharacterCard(int available ,int total)
    {
        quantityCharacterCardText.text = available + " / " + total;
    }
    public void CharacterCardButtonState(bool state)
    {
        characterCardButton.GetComponent<Image>().enabled = state;
    }
    public void ActionCardButtonState(bool state)
    {
        actionCardButton.GetComponent<Image>().enabled = state;
    }
    public void QuantityActionCard(int available, int total)
    {
        quantityActionCardText.text = available + " / " + total;
    }
    public void SaveData()
    {
        int characterCardSize = collectionManager.characterCardCount;
        int actionCardSize = collectionManager.actionCardCount;
        int characterCardMaxSize = playerManager.characterCardMaxSize;
        int actionCardMaxSize = playerManager.actionCardMaxSize;

        if (characterCardSize == characterCardMaxSize && actionCardSize == actionCardMaxSize)
        {
            playerManager.SaveCard();
            notificationManager.SetNewNotification("Deck Saved");
        }
        else if (characterCardSize != characterCardMaxSize && actionCardSize != actionCardMaxSize)
        {
            notificationManager.SetNewNotification("Your deck must have 3 character cards \n and 30 action cards");
        }
        else if (characterCardSize < characterCardMaxSize)
        {
            notificationManager.SetNewNotification("Your deck must have 3 character cards");
        }
        else if (actionCardSize < actionCardMaxSize)
        {
            notificationManager.SetNewNotification("Your deck must have 30 action cards");
        }
    }
    public void StartGame()
    {
        for(int i = 0; i < playerManager.characterCardDeckData.Count; i++)
        {
            CharacterCardData characterCardData = collectionManager.characterCardDataList[i];
            CharacterCardData characterCardDeckData = playerManager.characterCardDeckData[i];
            if(characterCardData != characterCardDeckData)
            {
                playerManager.deckSaved = false;
                break;
            }
        }
        for(int i = 0;i < playerManager.actionCardDeckData.Count; i++)
        {
            ActionCardData actionCardData = collectionManager.actionCardDataList[i];
            ActionCardData actionCardDeckData = playerManager.actionCardDeckData[i];
            if (actionCardData != actionCardDeckData)
            {
                playerManager.deckSaved = false;
                break;
            }
        }
        if(!playerManager.deckSaved)
        {
            notificationManager.SetNewNotification("Your deck has not been saved");
        }
        else
        {
            playerManager.ShuffleList(playerManager.actionCardDeckData);
            enemyManager.ShuffleList(enemyManager.actionCardDeckData);
            sceneChanger.SceneChange(SceneType.GamePlay);
        }

    }
    public void Return()
    {
        sceneChanger.SceneChange(SceneType.MainMenu);
    }
}
