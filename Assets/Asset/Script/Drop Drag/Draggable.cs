using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo = null;
    public Transform placeHolderParent = null;

    public GameObject placeHolderPrefab;
    public GameObject placeHolder;

    void Update()
    {
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        placeHolder = Instantiate(placeHolderPrefab, transform.parent);
        placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
        if (gameObject.GetComponent<CharacterCardDisplay>())
        {
            string characterName = gameObject.GetComponent<CharacterCardDisplay>().cardName;
            int maxHealth = gameObject.GetComponent<CharacterCardDisplay>().maxHealth;
            int skillPoint = gameObject.GetComponent<CharacterCardDisplay>().maxSkillPoint;
            Sprite cardSprite = gameObject.GetComponent<CharacterCardDisplay>().cardSprite;

            placeHolder.GetComponent<CharacterCardDisplay>().GetName(characterName);
            placeHolder.GetComponent<CharacterCardDisplay>().GetHealth(maxHealth);
            placeHolder.GetComponent<CharacterCardDisplay>().GetSkillPoint(skillPoint);
            placeHolder.GetComponent<CharacterCardDisplay>().GetImage(cardSprite);
        }
        parentToReturnTo = this.transform.parent;
        placeHolderParent = parentToReturnTo;

        Canvas canvas = FindObjectOfType<Canvas>();
        this.transform.SetParent(canvas.transform);

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        this.transform.position = Input.mousePosition;
        RectTransform rectTransform = transform.GetComponent<RectTransform>();

        // Đặt đối tượng lên đầu canvas
        rectTransform.SetAsLastSibling();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");
        // Đặt lại cha và vị trí của đối tượng
        this.transform.SetParent(parentToReturnTo);

        GetComponent<CanvasGroup>().blocksRaycasts = true;

        this.transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());
        Destroy(placeHolder);
    }
}
