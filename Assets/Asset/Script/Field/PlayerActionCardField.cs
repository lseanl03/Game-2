using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerActionCardField : MonoBehaviour, IPointerClickHandler
{
    public bool isZooming = false;
    public Animator animator;
    protected UIManager uiManager => UIManager.instance;
    protected GamePlayManager gamePlayManager => GamePlayManager.instance;
    public void Start()
    {
        animator = GetComponent<Animator>();
        InitialUnZoom();
    }
    public void Update()
    {
        if (isZooming)
        {
            if(CanEnableDraggableAndInfo()) EnableDraggableAndInfo(true);
        }
    }
    private bool CanEnableDraggableAndInfo()
    {
        return gamePlayManager.actionPhase;
    }
    private bool CannotDragCard()
    {
        return gamePlayManager.playerCanSwitchCharacterDying || !gamePlayManager.actionPhase ||
            gamePlayManager.currentTurn == TurnState.EnemyTurn || gamePlayManager.playerAttacking ||
            gamePlayManager.playerCanSwitchCharacterDying || gamePlayManager.enemyCanSwitchCharacterDying;
    }

    private bool CanPointerClick()
    {
        return uiManager.tutorialCanvas.isShowedActionCardTutorial && !isZooming;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (CanPointerClick()) ZoomState(true);
    }
    public void InitialUnZoom()
    {
        isZooming = false;
        EnableDraggableAndInfo(false);
    }
    public void ZoomState(bool state)
    {
        isZooming = state;
        animator.SetBool("Zoom", state);
        EnableDraggableAndInfo(state);
        uiManager.battleCanvas.skillPanel.SkillObj.SetActive(!state);
        uiManager.battleCanvas.informationPanel.playerSkillPointObj.SetActive(!state);
        uiManager.battleCanvas.informationPanel.enemySkillPointObj.SetActive(!state);

        if (isZooming)
        {
            AudioManager.instance.PlayOpenListActionCard();
            gamePlayManager.HideSelectIcon();

            if(gamePlayManager.actionPhase)
            uiManager.battleCanvas.switchCardBattlePanel.PanelState(false);
        }
    }
    public void CloseZoom()
    {
        if (isZooming)
        {
            isZooming = false;
            animator.SetBool("Zoom", false);
            EnableDraggableAndInfo(false);
            uiManager.battleCanvas.skillPanel.SkillObj.SetActive(true);
            uiManager.battleCanvas.informationPanel.playerSkillPointObj.SetActive(true);
            uiManager.battleCanvas.informationPanel.enemySkillPointObj.SetActive(true);
            AudioManager.instance.PlayCloseListActionCard();

        }
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
                    if (CannotDragCard()) child.GetComponent<ActionCardDragHover>().canDrag = false;
                }
            }
            if (child.GetComponent<CardInfo>())
            {
                child.GetComponent<CardInfo>().enabled = state;
            }
        }
    }
}
