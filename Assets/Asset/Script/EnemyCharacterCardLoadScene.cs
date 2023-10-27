using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacterCardLoadScene : MonoBehaviour
{
    protected EnemyManager enemyManager => EnemyManager.instance;
    private void Start()
    {
        for (int i = 0; i < enemyManager.characterCardDeckData.Count; i++)
        {
            CharacterCardLoadScene characterCardLoadScene = transform.GetChild(i).GetComponent<CharacterCardLoadScene>();
            characterCardLoadScene.cardImage.sprite = enemyManager.characterCardDeckData[i].cardSprite;
        }
    }
}
