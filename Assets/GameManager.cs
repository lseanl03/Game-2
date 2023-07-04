using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool startedGame = false;
    public bool waitingTime = true;

    public GameManager instance;
    public GameObject startGameText;

    public GameState gameState;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        UpdateGameState(GameState.SelectBattleCard);
        StartCoroutine(StartGame());
    }
    public void UpdateGameState(GameState gameState)
    {
        this.gameState = gameState;
        switch (gameState)
        {
            case GameState.SelectBattleCard:
                HandleSelectBattleCard();
                break;
            case GameState.PlayerTurn:
                break;
            case GameState.EnemyTurn:
                break;
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;
        }
    }

    private void HandleSelectBattleCard()
    {
        
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1);
        startGameText.SetActive(true);
        int randomTurn = Random.Range(0, 2);
        if(randomTurn == 0)
            startGameText.GetComponent<TextMeshProUGUI>().text = "Your Turn";
        else
            startGameText.GetComponent<TextMeshProUGUI>().text = "Enemy Turn";
        yield return new WaitForSeconds(1);
        startGameText.GetComponent<TextMeshProUGUI>().text = "Start Game";
        yield return new WaitForSeconds(1);
        startGameText.SetActive(false);
        startedGame = true;
    }
}
