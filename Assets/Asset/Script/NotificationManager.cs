using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class NotificationManager : MonoBehaviour
{
    public float countDown = 2f;
    public bool isShowing = false;
    public TextMeshProUGUI textMeshProUGUI;
    private void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        gameObject.SetActive(false);
    }
    private void Update()
    {
        if (isShowing)
        {
            StartCoroutine(Check());
        }
    }
    IEnumerator Check()
    {
        yield return new WaitForSeconds(countDown);
        HideObj();
    }
    public void GetTextNotification(string text)
    {
        textMeshProUGUI.text = text;
    }
    public void ShowObj()
    {
        isShowing = true;
        gameObject.SetActive(true);
    }
    public void HideObj()
    {
        isShowing = false;
        gameObject.SetActive(false);
    }
}
