using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public bool isYourTurn = true;
    public bool endRound = false;
    public int round;
    public int turnCount = 0;
    public int maxMana = 100;
    public int currentMana;

    public TextMeshProUGUI manaText;
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI roundText;

    private void Start()
    {
        currentMana = maxMana;
        turnCount = 2;
    }
    private void Update()
    {
        Turn();
        Mana();
    }
    public void EndTurn()
    {
        isYourTurn = !isYourTurn;
        turnCount++;
    }
    public void Turn()
    {
        //change turn
        if (isYourTurn)
        {
            turnText.text = "Your Turn";
        }
        else
        {
            turnText.text = "Opponent Turn";
        }
        //change round
        if (turnCount == 2)
        {
            endRound = true;
            turnCount = 0;
            round++;
            roundText.text = "Round " + round;
        }
        else
        {
            endRound = false;
        }
    }
    public void Mana()
    {
        if (endRound)
        {
            currentMana = maxMana;
        }
        else
        {
            if(currentMana < 0)
            {
                currentMana = 0;
            }
        }
        manaText.text = "" +currentMana;
    }
}
