using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    MainMenu,
    SelectCard,
    GamePlay
}
public class SceneChanger : MonoBehaviour
{
    public SceneType currentScene;

    public static SceneChanger instance;
    protected GameManager gameManager => GameManager.instance;
    protected PlayerManager deckManager => PlayerManager.instance;
    protected UIManager uIManager => UIManager.instance;
    protected CollectionManager collectionManager => CollectionManager.instance;
    protected NotificationManager notificationManager => NotificationManager.instance;
    public void Awake()
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
    private void Start()
    {
        SceneChange(currentScene);
    }
    public void SceneChange(SceneType sceneType)
    {
        currentScene = sceneType;
        switch (currentScene)
        {
            case SceneType.MainMenu:
                SceneManager.LoadScene("MainMenu");

                uIManager.selectCardCanvas.CanvasState(false);
                uIManager.battleCanvas.CanvasState(false);
                break; 
            case SceneType.SelectCard:
                SceneManager.LoadScene("SelectCard");

                if(collectionManager != null)
                {
                    collectionManager.optionalToolCanvas.CanvasState(false);
                    collectionManager.collectionCanvas.CanvasState(true);
                }
                 
                uIManager.selectCardCanvas.CanvasState(true);
                uIManager.battleCanvas.CanvasState(false);

                notificationManager.Reset();
                break; 
            case SceneType.GamePlay:
                SceneManager.LoadScene("GamePlay");

                uIManager.selectCardCanvas.CanvasState(false);
                uIManager.battleCanvas.CanvasState(true);

                notificationManager.Reset();
                break;

        }
    }
    public void OpenMainMenuScene()
    {
        SceneChange(SceneType.MainMenu);
    }
    public void OpenSelectCardScene()
    {
        
        SceneChange(SceneType.SelectCard);
    }
    public void OpenGamePlayScene()
    {
        SceneChange(SceneType.GamePlay);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
