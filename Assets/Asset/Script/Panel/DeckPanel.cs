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
        SetPlayerDeckText(playerManager.actionCardDeckData.Count.ToString());
        SetEnemyDeckText(enemyManager.actionCardDeckData.Count.ToString());
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
