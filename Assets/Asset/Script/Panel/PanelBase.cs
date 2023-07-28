using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBase : MonoBehaviour
{
    protected GamePlayManager gamePlayManager => GamePlayManager.instance;
    protected PlayerDeckManager playerDeckManager => PlayerDeckManager.instance;
    protected EnemyDeckManager enemyDeckManager => EnemyDeckManager.instance;
    protected NotificationManager notificationManager => NotificationManager.instance;
    public void PanelState(bool state)
    {
        gameObject.SetActive(state);
    }
}
