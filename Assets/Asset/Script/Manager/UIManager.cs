using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public SelectCardCanvas selectCardCanvas;
    public BattleCanvas battleCanvas;
    protected PlayerManager deckManager => PlayerManager.instance;
    protected CollectionManager collectionManager => CollectionManager.instance;
    protected NotificationManager notificationManager => NotificationManager.instance;
    protected TooltipManager tooltipManager => TooltipManager.instance;
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
    public void HideTooltip()
    {
        tooltipManager.tooltipCanvas.tooltipController.StateObj(false);
    }
}
