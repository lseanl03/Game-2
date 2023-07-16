using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public SelectCardCanvas selectCardCanvas;

    protected DeckManager deckManager => DeckManager.instance;

    protected CollectionManager collectionManager => CollectionManager.instance;

    protected NotificationManager notificationManager => NotificationManager.instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
}
