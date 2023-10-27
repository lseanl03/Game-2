using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCardBase : MonoBehaviour
{
    protected CollectionManager collectionManager => CollectionManager.instance;
    protected UIManager uIManager => UIManager.instance;
    protected GameManager gameManager => GameManager.instance;
    protected PlayerManager playerManager => PlayerManager.instance;
    protected NotificationManager notificationManager => NotificationManager.instance;
    protected SceneChanger sceneChanger => SceneChanger.instance;
    protected GamePlayManager gamePlayManager => GamePlayManager.instance;
}
