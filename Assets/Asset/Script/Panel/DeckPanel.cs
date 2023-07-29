using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckPanel : PanelBase
{
    public TextMeshProUGUI playerCardQuantityText;
    public TextMeshProUGUI enemyCardQuantityText;

    public void Start()
    {
        SetPlayerDeckText(playerDeckManager.actionCardDeckData.Count.ToString());
        SetEnemyDeckText(enemyDeckManager.actionCardDeckData.Count.ToString());
    }
    public void SetPlayerDeckText(string text)
    {
        playerCardQuantityText.text = text;
    }
    public void SetEnemyDeckText(string text)
    {
        enemyCardQuantityText.text = text;
    }
}
