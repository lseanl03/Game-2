using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerBase : MonoBehaviour
{
    protected EnemyManager enemyManager => EnemyManager.instance;
    protected PlayerManager playerManager => PlayerManager.instance;
    protected GamePlayManager gamePlayManager => GamePlayManager.instance;
    protected NotificationManager notificationManager => NotificationManager.instance;
    protected UIManager uiManager => UIManager.instance;
}
