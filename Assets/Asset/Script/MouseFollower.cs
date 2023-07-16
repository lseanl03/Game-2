using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    public Canvas canvas;
    public ActionCard actionCard;
    private void Awake()
    {
    }
    private void Start()
    {
    }
    void Update()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            Input.mousePosition,
            canvas.worldCamera,
            out position
                );
        transform.position = canvas.transform.TransformPoint(position);
        
    }
    public void Toggle(bool value)
    {
        gameObject.SetActive(value);
    }
}
