using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //public CharacterCardListData characterCardListData;
    public bool startedGame = false;
    public bool waitingTime = true;

    public GameState currentState;

    public static GameManager instance;
    protected CollectionManager collectionManager => CollectionManager.instance;
    protected UIManager uIManager => UIManager.instance;

    private void Awake()
    {
        if(instance == null)
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
    }
    public void UpdateGameState(GameState gameState)
    {
        currentState = gameState;
        switch (currentState)
        {
            case GameState.SelectFirstTurn:
                break;
            case GameState.SelectInitialCard:
                break;
            case GameState.SelectInitialBattleCard:
                break;
            case GameState.DrawCards:
                break;
            case GameState.PlayerTurn:
                break;
            case GameState.EnemyTurn:
                break;
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;
            default:
                Debug.Log("Out Case");
                break;
        }
    }
    public void CheckDeck()
    {
    }
    public void StartGame()
    {
        currentState = GameState.SelectFirstTurn;

    }
}
