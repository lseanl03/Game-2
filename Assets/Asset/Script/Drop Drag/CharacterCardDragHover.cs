using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterCardDragHover : MonoBehaviour, IPointerDownHandler
{
    protected GamePlayManager gamePlayManager => GamePlayManager.instance;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (gamePlayManager.startCombat)
        {
            Debug.Log("Down");

        } 
    }
}
