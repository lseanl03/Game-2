using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone2 : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("On Pointer Enter");
        if (eventData.pointerDrag != null)
        {
            Draggable2 draggable = eventData.pointerDrag.GetComponent<Draggable2>();
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
            Draggable2 draggable = eventData.pointerDrag.GetComponent<Draggable2>();
            if (draggable != null && draggable.placeHolderParent == this.transform)
            {
                draggable.placeHolderParent = draggable.parentToReturnTo;
            }
        }
    }
    public void OnDrop(PointerEventData eventData)
    {

        //Debug.Log(" was dropped on " + gameObject.name);
        Draggable2 draggable = eventData.pointerDrag.GetComponent<Draggable2>();
        if (draggable != null)
        {
            draggable.parentToReturnTo = this.transform;
        }
    }
}
