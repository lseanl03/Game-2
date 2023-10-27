using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDraggable : CardBase, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Parent Obj")]
    public Transform parentToReturnTo;
    public Transform placeHolderParent;


    [Header("Action Card Prefab")]
    public ActionCard actionCardHolderPrefab;
    public ActionCard actionCardHolderRootPrefab;
    public ActionCard actionCardHolder;
    public ActionCard actionCardHolderRoot;
    public ActionCard thisActionCard;

    [Header("Character Card Prefab")]
    public CharacterCard characterCardHolderPrefab;
    public CharacterCard characterCardHolderRootPrefab;
    private CharacterCard characterCardHolder;
    private CharacterCard characterCardHolderRoot;
    private CharacterCard thisCharacterCard;


    private GameObject cardListSelect;
    private GameObject cardListBattle;

    private Canvas canvas;

    private CharacterCard characterCard;
    private ActionCard actionCard;
    void Start()
    {
        cardListSelect = GameObject.FindGameObjectWithTag("CardListSelect");
        cardListBattle = GameObject.FindGameObjectWithTag("CardListBattle");
        actionCard = GetComponent<ActionCard>();
        characterCard = GetComponent<CharacterCard>();
        canvas = transform.root.GetComponentInChildren<Canvas>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        HandleBeginDrag();
    }
    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        HandleEndDrag();
    }
    public void HandleBeginDrag()
    {
        AudioManager.instance.PlayOpenListActionCard();
        transform.DOScale(1f, 0.1f);

        if (actionCard != null) ActionCardBeginDrag();
        else if (characterCard != null) CharacterCardBeginDrag();

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
            ActionCardEndDrag();
        }
        if (characterCard != null)
        {
            CharacterCardEndDrag();
        }
    }
    public void ActionCardBeginDrag()
    {
        if (transform.parent == cardListSelect.transform)
        {
            if (actionCard.quantityInDeck == 0)
            {
                thisActionCard = Instantiate(actionCard, transform.parent);
                thisActionCard.GetCardData(actionCard.actionCardData);
                thisActionCard.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
            }
            else if(actionCard.quantityInDeck > 0)
            {
                if (actionCardHolderRoot == null)
                {
                    actionCardHolderRoot = Instantiate(actionCardHolderRootPrefab, transform.parent);
                    actionCardHolderRoot.GetCardData(actionCard.actionCardData);
                    actionCardHolderRoot.quantityInDeck = actionCard.quantityInDeck;
                    actionCardHolderRoot.quantitySelectedText.text = actionCard.quantitySelectedText.text;
                    actionCardHolderRoot.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
                }
            }
            if (collectionManager.actionCardCount == playerManager.actionCardMaxSize)
            {
                notificationManager.SetNewNotification("Action cards is enough, can't add more");
            }
        }
        if (transform.parent == cardListBattle.transform)
        {
            if (actionCard.quantityInDeck > 1)
            {
                thisActionCard = Instantiate(actionCard, transform.parent);
                thisActionCard.GetCardData(actionCard.actionCardData);
                thisActionCard.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
            }
            else
            {
                if (actionCardHolder == null)
                {
                    actionCardHolder = Instantiate(actionCardHolderPrefab, transform.parent);
                    actionCardHolder.GetCardData(actionCard.actionCardData);
                    actionCardHolder.quantityInDeck = actionCard.quantityInDeck;
                    actionCardHolder.quantityInDeckText.text = actionCard.quantityInDeckText.text;
                    actionCardHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
                }
            }
        }
        actionCard.ManaState(false);
        actionCard.QuantitySelectedState(false);
        actionCard.QuantityInDeckState(false);
        actionCard.QuantityState(false);
    }
    public void CharacterCardBeginDrag()
    {
        if (transform.parent == cardListBattle.transform)
        {
            if (characterCardHolder == null)
            {
                characterCardHolder = Instantiate(characterCardHolderPrefab, transform.parent);
                characterCardHolder.GetOriginalCardInfo(characterCard.characterCardData);
                characterCardHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
            }
        }
        if (transform.parent == cardListSelect.transform)
        {
            if (characterCardHolderRoot == null)
            {
                characterCardHolderRoot = Instantiate(characterCardHolderRootPrefab, transform.parent);
                characterCardHolderRoot.GetOriginalCardInfo(characterCard.characterCardData);
                characterCardHolderRoot.QuantitySelectedObjState(true);
                characterCardHolderRoot.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
            }
            if (collectionManager.characterCardCount == playerManager.characterCardMaxSize)
            {
                notificationManager.SetNewNotification("Character cards is enough, can't add more");
            }
        }
        characterCard.HideObjects();
    }
    public void ActionCardEndDrag()
    {
        actionCard.ManaState(true);

        ActionCard[] actionCardList = cardListSelect.transform.GetComponentsInChildren<ActionCard>();
        if (transform.parent == cardListSelect.transform)
            actionCardList = cardListSelect.transform.GetComponentsInChildren<ActionCard>();
        else if (transform.parent == cardListBattle.transform)
            actionCardList = cardListBattle.transform.GetComponentsInChildren<ActionCard>();

        if (transform.parent == cardListSelect.transform)
        {
            this.actionCard.QuantityState(true);
            foreach (ActionCard card in actionCardList)
            {
                if (card.gameObject != gameObject && card.actionCardData == this.actionCard.actionCardData)
                {
                    if (this.actionCard.quantityInDeck > 0)
                    {
                        this.actionCard.QuantitySelectedState(true);
                        if (thisActionCard != null && this.actionCard.quantityInDeck > 1 || actionCardHolder != null)
                        {
                            this.actionCard.RecallCard(1);
                        }
                    }
                    if (this.actionCard.quantityInDeck == 0)
                    {
                        this.actionCard.QuantitySelectedState(false);
                    }
                    transform.SetSiblingIndex(card.transform.GetSiblingIndex());
                    Destroy(card.gameObject);
                    break;
                }
            }
            if (actionCardHolder != null)
            {
                Destroy(actionCardHolder.gameObject);
            }
        }
        else if (transform.parent == cardListBattle.transform)
        {
            foreach (ActionCard card in actionCardList)
            {
                if (card.gameObject != gameObject)
                {
                    if (card.actionCardData == this.actionCard.actionCardData) 
                    {
                        if (this.actionCard.quantityInDeck < 2 && actionCardHolder == null)
                        {
                            this.actionCard.AddCard(1);
                        }
                        transform.SetSiblingIndex(card.transform.GetSiblingIndex());
                        Destroy(card.gameObject);
                    }
                }
                else
                {
                    if (actionCardHolder == null && actionCardHolderRoot == null) this.actionCard.AddCard(1);
                }
            }
            this.actionCard.QuantityInDeckState(true);
        }

        if (actionCardHolderRoot != null)
        {
            actionCardHolderRoot.QuantitySelectedState(true);
            actionCardHolderRoot.quantityInDeck = this.actionCard.quantityInDeck;
            actionCardHolderRoot.quantityMaxInDeck = this.actionCard.quantityMaxInDeck;
            actionCardHolderRoot.quantitySelectedText.text = this.actionCard.quantitySelectedText.text;
            actionCardHolderRoot.quantityInDeckText.text = this.actionCard.quantityInDeckText.text;
        }
        if (thisActionCard != null)
        {
            if (thisActionCard.transform.parent == cardListSelect.transform)
                thisActionCard.QuantitySelectedState(true);
            if (thisActionCard.transform.parent == cardListBattle.transform)
                thisActionCard.QuantitySelectedState(false);

            thisActionCard.quantityInDeck = actionCard.quantityInDeck;
            thisActionCard.quantityMaxInDeck = actionCard.quantityMaxInDeck;
            thisActionCard.quantitySelectedText.text = actionCard.quantitySelectedText.text;
            thisActionCard.quantityInDeckText.text = actionCard.quantityInDeckText.text;
        }
    }
    public void CharacterCardEndDrag()
    {
        characterCard.ShowObjects();

        if (characterCardHolderRoot != null)
        {
            if (this.transform.parent == characterCardHolderRoot.transform.parent)
            {
                Destroy(characterCardHolderRoot.gameObject);
                this.transform.SetSiblingIndex(characterCardHolderRoot.transform.GetSiblingIndex());
                characterCardHolderRoot.RecallCard(1);
            }
            else
            {
                characterCardHolderRoot.AddCard(1);
                characterCardHolderRoot.QuantitySelectedObjState(true);
            }
        }

        if (characterCardHolder != null)
        {
            Destroy(characterCardHolder.gameObject);
            if (this.transform.parent == characterCardHolder.transform.parent)
            {
                this.transform.SetSiblingIndex(characterCardHolder.transform.GetSiblingIndex());
            }
        }
    }

}
