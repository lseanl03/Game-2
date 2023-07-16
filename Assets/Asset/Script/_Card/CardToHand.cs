using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardToHand : MonoBehaviour
{
    public GameObject handPanel;
    private void Start()
    {
        handPanel = GameObject.Find("HandPanel");
        gameObject.transform.SetParent(handPanel.transform);
        gameObject.transform.localScale = Vector3.one;
    }
    private void Update()
    {
    }
}
