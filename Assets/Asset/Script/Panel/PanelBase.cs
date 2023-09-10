using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBase : MonoBehaviour
{
    protected GamePlayManager gamePlayManager => GamePlayManager.instance;
    protected PlayerManager playerManager => PlayerManager.instance;
    protected EnemyManager enemyManager => EnemyManager.instance;
    protected NotificationManager notificationManager => NotificationManager.instance;
    protected UIManager uiManager => UIManager.instance;
    protected TooltipManager tooltipManager => TooltipManager.instance;
    protected SceneChanger sceneChanger => SceneChanger.instance;
    public void PanelState(bool state)
    {
        gameObject.SetActive(state);
    }
}
