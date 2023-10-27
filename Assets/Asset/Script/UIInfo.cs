using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInfo : PanelBase
{
    public TextMeshProUGUI cardQuantityText;
    public TextMeshProUGUI actionPointText;
    public Transform skillPoint;
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
        for(int i = 0;i < skillPoint.childCount; i++)
        {
            skillPoint.GetChild(i).gameObject.SetActive(i < value);
            if (!skillPoint.GetChild(i).gameObject.activeSelf)
            {
                skillPoint.GetChild(i).GetComponent<SkillPoint>().scaleAction = true;
            }
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
