using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : SelectCardBase, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo = null;
    public Transform placeHolderParent = null;

    public GameObject placeHolderPrefab;
    public GameObject placeHolderRootPrefab;

    public GameObject placeHolderRoot;
    public GameObject placeHolder;
    public GameObject thisObj;

    public GameObject cardListSelectParent;
    public GameObject cardListBattleParent;
    public GameObject cardListSelect;
    public GameObject cardListBattle;

    public ActionCard actionCard;
    public CharacterCard characterCard;
    private Canvas canvas;

    void Start()
    {
        cardListSelectParent = GameObject.FindGameObjectWithTag("CardListSelect");
        cardListBattleParent = GameObject.FindGameObjectWithTag("CardListBattle");
        cardListSelect = cardListSelectParent;
        cardListBattle = cardListBattleParent;
        actionCard = GetComponent<ActionCard>();
        characterCard = GetComponent<CharacterCard>();
        canvas = transform.root.GetComponentInChildren<Canvas>();
    }
    void Update()
    {
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBegin");
        HandleBeginDrag();

    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");

        this.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");
        HandleEndDrag();
    }
    public void HandleBeginDrag()
    {
        if (actionCard != null)
        {
            if (transform.parent == cardListSelect.transform)
            {
                if (actionCard.remainingQuantity > 1)
                {
                    thisObj = Instantiate(gameObject, transform.parent);
                    thisObj.GetComponent<ActionCard>().GetOriginalCardInfo(actionCard.actionCardData);
                    thisObj.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
                }
                else
                {
                    if (placeHolderRoot == null)
                    {
                        placeHolderRoot = Instantiate(placeHolderRootPrefab, transform.parent);
                        placeHolderRoot.GetComponent<ActionCard>().GetOriginalCardInfo(actionCard.actionCardData);
                        placeHolderRoot.GetComponent<ActionCard>().quantitySelected = actionCard.quantitySelected;
                        placeHolderRoot.GetComponent<ActionCard>().quantitySelectedText.text = actionCard.quantitySelectedText.text;
                        placeHolderRoot.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
                    }
                }
                if (collectionManager.actionCardCount == playerDeckManager.actionCardMaxSize)
                {
                    notificationManager.SetNewNotification("Action cards is enough, can't add more");
                }
            }
            if (transform.parent == cardListBattle.transform)
            {
                if (actionCard.quantitySelected > 1)
                {
                    thisObj = Instantiate(gameObject, transform.parent);
                    thisObj.GetComponent<ActionCard>().GetOriginalCardInfo(actionCard.actionCardData);
                    thisObj.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
                }
                else
                {
                    if (placeHolder == null)
                    {
                        placeHolder = Instantiate(placeHolderPrefab, transform.parent);
                        placeHolder.GetComponent<ActionCard>().GetOriginalCardInfo(actionCard.actionCardData);
                        placeHolder.GetComponent<ActionCard>().quantitySelected = actionCard.quantitySelected;
                        placeHolder.GetComponent<ActionCard>().quantityInDeckText.text = actionCard.quantityInDeckText.text;
                        placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
                    }
                }
            }

            actionCard.ManaState(false);
            actionCard.QuantitySelectedState(false);
            actionCard.QuantityInDeckState(false);
            actionCard.QuantityState(false);

        }

        if (characterCard != null)
        {
            if (transform.parent == cardListBattle.transform)
            {
                if (placeHolder == null)
                {
                    placeHolder = Instantiate(placeHolderPrefab, transform.parent);
                    placeHolder.GetComponent<CharacterCard>().GetOriginalCardInfo(characterCard.characterCardData);
                    placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
                }
            }
            if (transform.parent == cardListSelect.transform)
            {
                if (placeHolderRoot == null)
                {
                    placeHolderRoot = Instantiate(placeHolderRootPrefab, transform.parent);
                    placeHolderRoot.GetComponent<CharacterCard>().GetOriginalCardInfo(characterCard.characterCardData);
                    placeHolderRoot.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
                }
                if(collectionManager.characterCardCount == playerDeckManager.characterCardMaxSize)
                {
                    notificationManager.SetNewNotification("Character cards is enough, can't add more");
                }
            }
            characterCard.HideObjects();
        }
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        parentToReturnTo = this.transform.parent;
        placeHolderParent = parentToReturnTo;
        this.transform.SetParent(canvas.transform);
    }
    public void HandleEndDrag()
    {
        this.transform.SetParent(parentToReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (actionCard != null)
        {
            actionCard.ManaState(false);

            //-------------------------------------

            if (transform.parent == cardListSelect.transform)
            {
                this.actionCard.QuantityState(true);
                ActionCard[] actionCard = cardListSelect.transform.GetComponentsInChildren<ActionCard>();
                foreach (ActionCard card in actionCard)
                {
                    if (card.gameObject != gameObject)
                    {
                        if (card.actionCardData== this.actionCard.actionCardData)
                        {
                            if (this.actionCard.quantitySelected > 0)
                            {
                                if (thisObj != null && this.actionCard.quantitySelected > 1)
                                {
                                    if (placeHolder == null)
                                    {
                                        this.actionCard.RecallCard(1);
                                    }
                                }
                                if (placeHolderRoot == null && placeHolder == null)
                                {
                                    this.actionCard.AddCard(1);
                                }
                                if (placeHolder != null)
                                {
                                    this.actionCard.RecallCard(1);
                                }
                                if (this.actionCard.quantitySelected > 0)
                                {
                                    this.actionCard.QuantitySelectedState(true);
                                }
                                else
                                {
                                    this.actionCard.QuantitySelectedState(false);
                                }
                            }
                            transform.SetSiblingIndex(card.transform.GetSiblingIndex());
                            Destroy(card.gameObject);
                        }
                    }
                }
                if (placeHolder != null)
                {
                    Destroy(placeHolder);
                }
            }

            //-------------------------------------

            if (transform.parent == cardListBattle.transform)
            {
                ActionCard[] actionCard = cardListBattle.transform.GetComponentsInChildren<ActionCard>();
                foreach (ActionCard card in actionCard)
                {
                    if (card.gameObject != gameObject) // kiểm tra nếu trong cardListBattle có thẻ nào khác thẻ này
                    {
                        if (card.actionCardData == this.actionCard.actionCardData) // kiểm tra nếu có thẻ giống thẻ này 
                        {
                            if (this.actionCard.quantitySelected < 2) // kiểm tra nếu thẻ này có số lượng thẻ đã chọn ít hơn 2
                            {
                                if (placeHolder == null)
                                {
                                    this.actionCard.AddCard(1);
                                }
                            }
                            else
                            {

                            }
                            transform.SetSiblingIndex(card.transform.GetSiblingIndex());
                            Destroy(card.gameObject);
                        }
                        else
                        {
                        }
                    }
                    else // kiểm tra nếu trong cardListBattle có thẻ này
                    {
                        if (placeHolder == null && placeHolderRoot == null)
                        {
                            this.actionCard.AddCard(1);
                        }
                    }
                }
                this.actionCard.QuantityInDeckState(true);

                if (placeHolderRoot != null)
                {
                    //Debug.Log(placeHolderRoot);
                    placeHolderRoot.GetComponent<ActionCard>().QuantitySelectedState(true);
                    placeHolderRoot.GetComponent<ActionCard>().quantitySelected = this.actionCard.quantitySelected;
                    placeHolderRoot.GetComponent<ActionCard>().remainingQuantity = this.actionCard.remainingQuantity;
                    placeHolderRoot.GetComponent<ActionCard>().quantitySelectedText.text = this.actionCard.quantitySelectedText.text;
                    placeHolderRoot.GetComponent<ActionCard>().quantityInDeckText.text = this.actionCard.quantityInDeckText.text;
                }
            }
            if (thisObj != null)
            {
                //Debug.Log(thisObj);
                if (thisObj.transform.parent == cardListSelect.transform)
                    thisObj.GetComponent<ActionCard>().QuantitySelectedState(true);
                if (thisObj.transform.parent == cardListBattle.transform)
                    thisObj.GetComponent<ActionCard>().QuantitySelectedState(false);

                thisObj.GetComponent<ActionCard>().quantitySelected = actionCard.quantitySelected;
                thisObj.GetComponent<ActionCard>().remainingQuantity = actionCard.remainingQuantity;
                thisObj.GetComponent<ActionCard>().quantitySelectedText.text = actionCard.quantitySelectedText.text;
                thisObj.GetComponent<ActionCard>().quantityInDeckText.text = actionCard.quantityInDeckText.text;
            }
        }


        if (characterCard != null)
        {
            characterCard.ShowObjects();

            if (placeHolderRoot != null)
            {
                if (transform.parent == placeHolderRoot.transform.parent)
                {
                    Destroy(placeHolderRoot);
                    this.transform.SetSiblingIndex(placeHolderRoot.transform.GetSiblingIndex());
                }
            }

            if (placeHolder != null)
            {
                Destroy(placeHolder);
                this.transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());
            }
            if(transform.parent == cardListSelect.transform)
            {
                if (characterCard.quantitySelected > 0)
                    characterCard.RecallCard(1);
            }
            if(transform.parent == cardListBattle.transform)
            {
                if (characterCard.quantitySelected == 0)
                    characterCard.AddCard(1);
            }
        }
    }

}
