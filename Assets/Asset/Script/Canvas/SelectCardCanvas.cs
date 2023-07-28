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
        int characterCardMaxSize = playerDeckManager.characterCardMaxSize;
        int actionCardMaxSize = playerDeckManager.actionCardMaxSize;

        if (characterCardSize == characterCardMaxSize && actionCardSize == actionCardMaxSize)
        {
            playerDeckManager.SaveCard();
            notificationManager.SetNewNotification("Deck saved");
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
        for(int i = 0; i < playerDeckManager.characterCardDeckData.Count; i++)
        {
            CharacterCardData characterCardData = collectionManager.characterCardDataList[i];
            CharacterCardData characterCardDeckData = playerDeckManager.characterCardDeckData[i];
            if(characterCardData != characterCardDeckData)
            {
                playerDeckManager.deckSaved = false;
                break;
            }
        }
        for(int i = 0;i < playerDeckManager.actionCardDeckData.Count; i++)
        {
            ActionCardData actionCardData = collectionManager.actionCardDataList[i];
            ActionCardData actionCardDeckData = playerDeckManager.actionCardDeckData[i];
            if (actionCardData != actionCardDeckData)
            {
                playerDeckManager.deckSaved = false;
                break;
            }
        }
        if(!playerDeckManager.deckSaved)
        {
            notificationManager.SetNewNotification("Your deck has not been saved");
        }
        else
        {
            sceneChanger.SceneChange(SceneType.GamePlay);
        }

    }
    public void Return()
    {
        sceneChanger.SceneChange(SceneType.MainMenu);
    }
}
