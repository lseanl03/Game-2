using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [Header("Action Point")]
    public int initialActionPoint = 100;
    public int currentActionPoint;

    [Header("Skill Point")]
    public int initialSkillPoint = 5;
    public int currentSkillPoint;

    [Header("Deck Data")]
    public int characterCardMaxSize = 3;
    public int actionCardMaxSize = 30;
    public List<CharacterCardData> characterCardDeckData;
    public List<ActionCardData> actionCardDeckData;

    [Header("Action Card Taken Data")]
    public List<ActionCardData> actionCardTakenDataList;
    
    protected CollectionManager collectionManager => CollectionManager.instance;
    protected UIManager uiManager => UIManager.instance;
    protected NotificationManager notificationManager => NotificationManager.instance;
    public virtual void Start()
    {
        Refresh();
    }
    public void ShuffleDeck()
    {
        if (actionCardDeckData != null)
        {
            ShuffleList(actionCardDeckData);
        }
    }
    public void Refresh()
    {
        ResetActionPoint();
        ResetSkillPoint();
        ShuffleDeck();
    }
    public void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    public void ConsumeActionPoint(int value)
    {
        if(currentActionPoint - value < 0)
        {
            notificationManager.SetNewNotification("Current action points are not enough");
            return;
        }
        currentActionPoint -= value;
    }
    public void ConsumeSkillPoint(int value)
    {
        if(currentSkillPoint - value < 0)
        {
            notificationManager.SetNewNotification("Current skill points are not enough");
            return;
        }
        currentSkillPoint -= value;
        if(currentSkillPoint >= initialSkillPoint)
        {
            currentSkillPoint = initialSkillPoint;
        }
    }
    public void RecoveryActionPoint(int value)
    {
        currentActionPoint += value;
    }
    public void RecoverySkillPoint(int value)
    {
        currentSkillPoint += value;
        if (currentSkillPoint >= initialSkillPoint)
        {
            currentSkillPoint = initialSkillPoint;
        }
    }
    public void ResetActionPoint()
    {
        currentActionPoint = initialActionPoint;
    }
    public void ResetSkillPoint()
    {
        currentSkillPoint = 3;
    }
}
