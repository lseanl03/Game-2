using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
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
            Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
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
            if(cardListBattle != null)
            {
                if (cardListBattle.thisCardType == CardSelectType.CharacterCard)
                {
                    CharacterCard characterCard = eventData.pointerDrag.GetComponent<CharacterCard>();
                    cardListBattle.collectionManager.characterCardListBattle.Add(characterCard.characterCardAndQuantity);
                }
                if (cardListBattle.thisCardType == CardSelectType.ActionCard)
                {
                    ActionCard actionCard = eventData.pointerDrag.GetComponent<ActionCard>();
                    cardListBattle.collectionManager.actionCardListBattle.Add(actionCard.actionCardAndQuantity);
                }
            }
            if(cardListSelect != null)
            {
                if (cardListSelect.thisCardType == CardSelectType.CharacterCard)
                {
                    CharacterCard characterCard = eventData.pointerDrag.GetComponent<CharacterCard>();
                    cardListSelect.collectionManager.characterCardListBattle.Remove(characterCard.characterCardAndQuantity);
                }
                if (cardListSelect.thisCardType == CardSelectType.ActionCard)
                {
                    ActionCard actionCard = eventData.pointerDrag.GetComponent<ActionCard>();
                    cardListSelect.collectionManager.actionCardListBattle.Remove(actionCard.actionCardAndQuantity);
                }
            }
        }
    }
}
