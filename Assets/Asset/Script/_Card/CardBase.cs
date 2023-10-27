using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBase : MonoBehaviour
{
    protected GamePlayManager gamePlayManager => GamePlayManager.instance;
    protected UIManager uiManager => UIManager.instance;
    protected TooltipManager tooltipManager => TooltipManager.instance;
    protected PlayerManager playerManager => PlayerManager.instance;
    protected EnemyManager enemyManager => EnemyManager.instance;
    protected NotificationManager notificationManager => NotificationManager.instance;
    protected CollectionManager collectionManager => CollectionManager.instance;

}
