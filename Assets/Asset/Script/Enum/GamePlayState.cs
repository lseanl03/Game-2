using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePlayState
{
    SelectFirstTurn,
    SelectInitialActionCard,
    SelectInitialBattleCharacterCard,
    DrawCards,
    Victory,
    Lose
}
public enum TurnState
{
    YourTurn,
    EnemyTurn
}
