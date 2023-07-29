using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationPanel : PanelBase
{
    public TextMeshProUGUI playerManaText;
    public TextMeshProUGUI enemyManaText;
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI roundText;
    public Button endTurnButton;

    public void Start()
    {
        SetPlayerManaText(gamePlayManager.InitialPlayerMana.ToString());
        SetEnemyManaText(gamePlayManager.InitialEnemyMana.ToString());
    }
    public void SetPlayerManaText(string text)
    {
        playerManaText.text = text;
    }
    public void SetEnemyManaText(string text)
    {
        playerManaText.text = text;
    }
}
