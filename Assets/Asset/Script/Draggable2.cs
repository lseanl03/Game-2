using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable2 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo = null;
    public Transform placeHolderParent = null;

    public GameObject placeHolderPrefab;
    private GameObject placeHolder;

    private Camera cam;
    private void Awake()
    {
        cam = Camera.main;
    }
    void Update()
    {

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        placeHolder = Instantiate(placeHolderPrefab, transform.parent);
        placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
        if(gameObject.GetComponent<CharacterCard>())
        {
            //string characterName = gameObject.GetComponent<CharacterCardDisplay>().cardName;
            //string description = gameObject.GetComponent<CharacterCardDisplay>().cardDescription;
            //int maxHealth = gameObject.GetComponent<CharacterCardDisplay>().maxHealth;
            //int skillPoint = gameObject.GetComponent<CharacterCardDisplay>().maxSkillPoint;
            //Sprite cardSprite = gameObject.GetComponent<CharacterCardDisplay>().cardSprite;
            //CharacterCard card = new CharacterCard(characterName, description, maxHealth, skillPoint);
            //placeHolder.GetComponent<CharacterCardDisplay>().GetName(characterName);
            //placeHolder.GetComponent<CharacterCardDisplay>().GetHealth(maxHealth);
            //placeHolder.GetComponent<CharacterCardDisplay>().GetSkillPoint(skillPoint);
            //placeHolder.GetComponent<CharacterCardDisplay>().GetImage(cardSprite);
        }
        parentToReturnTo = this.transform.parent;
        placeHolderParent = parentToReturnTo;
        this.transform.SetParent(this.transform.parent.parent.parent.parent); //canvas

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        Vector3 mousePosition = cam.ScreenToWorldPoint(eventData.position);
        mousePosition.z = 0; 
        this.transform.position = mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");
        // Đặt lại cha và vị trí của đối tượng
        this.transform.SetParent(parentToReturnTo);
        this.transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());

        GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(placeHolder);
    }
}
