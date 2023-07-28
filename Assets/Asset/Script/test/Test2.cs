using UnityEngine;
using UnityEngine.EventSystems;

public class Test2 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalPosition;
    private bool isMouseOver = false;
    private float hoverOffset = 20f;

    private void Start()
    {
        originalPosition = transform.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }

    private void Update()
    {
        if (isMouseOver)
        {
            transform.position = originalPosition + Vector3.up * hoverOffset;
            transform.SetAsLastSibling();
        }

    }
}
