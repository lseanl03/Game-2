using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool haveCharacterCard = false;
    public void Update()
    {
        if(transform.childCount != 0)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<Draggable>())
                {
                    haveCharacterCard = true;
                    break;
                }
                else
                {
                    haveCharacterCard = false;
                }
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("On Pointer Enter");
        if(eventData.pointerDrag != null)
        {
            Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
            if (draggable != null)
            {
                draggable.placeHolderParent = this.transform;
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("On Pointer Exit");
        if (eventData.pointerDrag != null)
        {
            Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
            if (draggable != null && draggable.placeHolderParent == this.transform)
            {
                draggable.placeHolderParent = draggable.parentToReturnTo;
            }
        }
    }
    public void OnDrop(PointerEventData eventData)
    {

        //Debug.Log(" was dropped on " + gameObject.name);
        Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
        if (draggable != null)
        {
            draggable.parentToReturnTo = this.transform;
        }
    }
}
