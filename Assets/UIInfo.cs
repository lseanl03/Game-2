using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInfo : PanelBase
{
    public TextMeshProUGUI cardQuantityText;
    public TextMeshProUGUI actionPointText;
    public List<GameObject> skillPointList;
    public PlayerType playerType;
    private void Update()
    {
        if(playerType == PlayerType.Player)
        {
            SetActionPointText(playerManager.currentActionPoint);
            SetSkillPoint(playerManager.currentSkillPoint);
            SetCardInDeckText(playerManager.actionCardDeckData.Count);
        }
        if(playerType == PlayerType.Enemy)
        {
            SetActionPointText(enemyManager.currentActionPoint);
            SetSkillPoint(enemyManager.currentSkillPoint);
            SetCardInDeckText(enemyManager.actionCardDeckData.Count);
        }
    }
    public void SetSkillPoint(int value)
    {
        for(int i = 0; i < skillPointList.Count; i++)
        {
            skillPointList[i].SetActive(i<value);
        }
    }
    public void SetActionPointText(int value)
    {
        actionPointText.text = value.ToString();
    }
    public void SetCardInDeckText(int value)
    {
        cardQuantityText.text = value.ToString();
    }
}
