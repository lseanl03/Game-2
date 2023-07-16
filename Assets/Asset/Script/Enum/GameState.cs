using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    SelectFirstTurn,
    SelectInitialCard,
    SelectInitialBattleCard,
    DrawCards,
    PlayerTurn,
    EnemyTurn,
    Victory,
    Lose
}
