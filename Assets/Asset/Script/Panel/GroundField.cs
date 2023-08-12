using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GroundField : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected GamePlayManager gamePlayManager => GamePlayManager.instance;
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("On Pointer Enter");
        if (eventData.pointerDrag != null)
        {
            ActionCardDragHover actionCardDrag = eventData.pointerDrag.GetComponent<ActionCardDragHover>();
            if (actionCardDrag != null)
            {
                actionCardDrag.placeHolderParent = this.transform;
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("On Pointer Exit");
        if (eventData.pointerDrag != null)
        {
            ActionCardDragHover actionCardDrag = eventData.pointerDrag.GetComponent<ActionCardDragHover>();
            if (actionCardDrag != null && actionCardDrag.placeHolderParent == this.transform)
            {
                actionCardDrag.placeHolderParent = actionCardDrag.parentToReturn;
            }
        }
    }
    public void OnDrop(PointerEventData eventData)
    {

        //Debug.Log(" was dropped on " + gameObject.name);
        ActionCardDragHover actionCardDrag = eventData.pointerDrag.GetComponent<ActionCardDragHover>();
        if (actionCardDrag != null)
        {
            actionCardDrag.parentToReturn = this.transform;
        }
    }
}
