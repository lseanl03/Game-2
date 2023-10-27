using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    MainMenu,
    SelectCard,
    GamePlay,
    Tutorial
}
public class SceneChanger : MonoBehaviour
{
    public SceneType currentScene;

    public static SceneChanger instance;
    protected GameManager gameManager => GameManager.instance;
    protected PlayerManager playerManager => PlayerManager.instance;
    protected EnemyManager enemyManager => EnemyManager.instance;
    protected UIManager uIManager => UIManager.instance;
    protected CollectionManager collectionManager => CollectionManager.instance;
    protected NotificationManager notificationManager => NotificationManager.instance;
    protected GamePlayManager gamePlayManager => GamePlayManager.instance;
    protected AudioManager audioManager => AudioManager.instance;
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
    public IEnumerator SceneChange(SceneType sceneType)
    {
        notificationManager.ResetText();
        yield return StartCoroutine(uIManager.Fade(true));
        currentScene = sceneType;
        switch (currentScene)
        {
            case SceneType.MainMenu:
                SceneManager.LoadScene("MainMenu");

                audioManager.PlayTheme();
                StartCoroutine(uIManager.Fade(false));

                collectionManager.collectionCanvas.CanvasState(false);

                uIManager.selectCardCanvas.CanvasState(false);
                uIManager.battleCanvas.selectTurnPanel.PanelState(false);
                uIManager.battleCanvas.selectInitialActionCardPanel.PanelState(false);
                uIManager.battleCanvas.settingPanel.PanelState(true);

                playerManager.Refresh();
                enemyManager.Refresh();


                break; 
            case SceneType.SelectCard:
                SceneManager.LoadScene("SelectCard");

                StartCoroutine(uIManager.Fade(false));

                collectionManager.collectionCanvas.CanvasState(true);
                collectionManager.optionalToolCanvas.CanvasState(false);

                uIManager.selectCardCanvas.CanvasState(true);
                uIManager.battleCanvas.settingPanel.PanelState(false);

                break;
            case SceneType.Tutorial:
                SceneManager.LoadScene("Tutorial");

                audioManager.PlayCombatTheme();
                StartCoroutine(uIManager.Fade(false));

                uIManager.battleCanvas.settingPanel.PanelState(false);
                uIManager.tutorialCanvas.CanvasState(true);
                uIManager.tutorialCanvas.ResetIsUsed(false);
                uIManager.battleCanvas.CanvasState(true);
                break;
            case SceneType.GamePlay:

                audioManager.PlayCombatTheme();
                StartCoroutine(uIManager.Fade(false));
                collectionManager.collectionCanvas.CanvasState(false);

                uIManager.tutorialCanvas.CanvasState(false);
                uIManager.tutorialCanvas.ResetIsUsed(true);
                uIManager.selectCardCanvas.CanvasState(false);
                uIManager.battleCanvas.CanvasState(true);

                break;

        }
    }
    public void OpenMainMenuScene()
    {
        StartCoroutine(SceneChange(SceneType.MainMenu));
    }
    public void OpenSelectCardScene()
    {
        StartCoroutine(SceneChange(SceneType.SelectCard));
    }
    public void OpenGamePlayScene()
    {
        StartCoroutine(SceneChange(SceneType.GamePlay));
    }
    public void OpenTutorialScene()
    {
        StartCoroutine(SceneChange(SceneType.Tutorial));
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
