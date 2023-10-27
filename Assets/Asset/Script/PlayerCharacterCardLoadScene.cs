using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterCardLoadScene : MonoBehaviour
{
    protected PlayerManager playerManager => PlayerManager.instance;
    private void Start()
    {
        for(int i=0;i<playerManager.characterCardDeckData.Count;i++)
        {
            CharacterCardLoadScene characterCardLoadScene = transform.GetChild(i).GetComponent<CharacterCardLoadScene>();
            characterCardLoadScene.cardImage.sprite = playerManager.characterCardDeckData[i].cardSprite;
        }
    }
}
