using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerActionCardField : MonoBehaviour, IPointerClickHandler
{
    public bool isZooming = false;
    public RectTransform rectTransform;
    public Animator animator;
    protected UIManager uiManager => UIManager.instance;
    protected GamePlayManager gamePlayManager => GamePlayManager.instance;
    public void Start()
    {
        animator = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
        InitialUnZoom(false);
    }
    public void Update()
    {
        if (isZooming)
        {
            if(gamePlayManager.playerSelectedCharacterBattleInitial && 
                gamePlayManager.currentTurn == TurnState.YourTurn || 
                gamePlayManager.currentState == GamePlayState.ActionPhase ||
                gamePlayManager.currentState == GamePlayState.SelectBattleCharacter)
            {
                EnableDraggableAndInfo(true);
            }
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        ZoomState(true);
    }
    public void InitialUnZoom(bool state)
    {
        isZooming = state;
        EnableDraggableAndInfo(state);
    }
    public void ZoomState(bool state)
    {
        isZooming = state;
        animator.SetBool("Zoom", state);
        EnableDraggableAndInfo(state);
    }
    public void EnableDraggableAndInfo(bool state)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.GetComponent<ActionCardDragHover>())
            {
                child.GetComponent<ActionCardDragHover>().canDrag = state;
                child.GetComponent<ActionCardDragHover>().canPush = state;

                if (state == true)
                {
                    if (!gamePlayManager.playerSelectedCharacterBattleInitial || 
                        gamePlayManager.playerCanSwitchCharacterDying ||
                        !gamePlayManager.actionPhase ||
                        gamePlayManager.currentTurn == TurnState.EnemyTurn)
                        child.GetComponent<ActionCardDragHover>().canDrag = false;
                }
            }
            if (child.GetComponent<CardInfo>())
            {
                child.GetComponent<CardInfo>().enabled = state;
            }
        }
    }
}
