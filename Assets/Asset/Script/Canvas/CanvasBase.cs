using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasBase : MonoBehaviour
{
    public void CanvasState(bool state)
    {
        gameObject.SetActive(state);
    }
    protected GamePlayManager gamePlayManager => GamePlayManager.instance;
    protected CollectionManager collectionManager => CollectionManager.instance;
    protected NotificationManager notificationManager => NotificationManager.instance;
    protected SceneChanger sceneChanger => SceneChanger.instance;
    protected UIManager uiManager => UIManager.instance;
    protected GameManager gameManager => GameManager.instance;
    protected PlayerManager playerManager => PlayerManager.instance;
    protected EnemyManager enemyManager => EnemyManager.instance;
}
