using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CardListBattle cardListBattle;
    public CardListSelect cardListSelect;
    void Start()
    {
        cardListBattle = GetComponent<CardListBattle>();
        cardListSelect = GetComponent<CardListSelect>();
    }
    public void Update()
    {

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("On Pointer Enter");
        if(eventData.pointerDrag != null)
        {
            CardDraggable draggable = eventData.pointerDrag.GetComponent<CardDraggable>();
            if (draggable != null)
            {
                draggable.placeHolderParent = this.transform;
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("On Pointer Exit");
        if (eventData.pointerDrag != null)
        {
            CardDraggable draggable = eventData.pointerDrag.GetComponent<CardDraggable>();
            if (draggable != null && draggable.placeHolderParent == this.transform)
            {
                draggable.placeHolderParent = draggable.parentToReturnTo;
            }
        }
    }
    public void OnDrop(PointerEventData eventData)
    {

        //Debug.Log(" was dropped on " + gameObject.name);
        CardDraggable draggable = eventData.pointerDrag.GetComponent<CardDraggable>();
        if (draggable != null)
        {
            draggable.parentToReturnTo = this.transform;
        }
    }
}
