using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePlayState
{
    SelectFirstTurn,
    SelectInitialActionCard,
    SelectInitialBattleCharacterCard,
    DrawCards,
    YourTurn,
    EnemyTurn,
    Victory,
    Lose
}
