using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    public int characterCardCount = 0;
    public int actionCardCount = 0;

    public CollectionCanvas collectionCanvas;
    public OptionalToolCanvas optionalToolCanvas;

    public List<CharacterCardData> characterCardDataList;
    public List<ActionCardData> actionCardDataList;

    public static CollectionManager instance;
    protected UIManager uIManager => UIManager.instance;
    protected GameManager gameManager => GameManager.instance;
    protected PlayerManager playerManager => PlayerManager.instance;
    protected NotificationManager notificationManager => NotificationManager.instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Update()
    {
        characterCardCount = characterCardDataList.Count;
        actionCardCount = actionCardDataList.Count;
        uIManager.selectCardCanvas.QuantityCharacterCard(characterCardCount, playerManager.characterCardMaxSize);
        uIManager.selectCardCanvas.QuantityActionCard(actionCardCount, playerManager.actionCardMaxSize);
    }
}
