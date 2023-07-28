using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private string tempMessage = string.Empty;
    [SerializeField] public float resetTextTime;
    public TextMeshProUGUI notificationText;
    public Animator animator;

    IEnumerator NotificationCoroutine;

    public static NotificationManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public void Start()
    {
    }
    public void SetNewNotification(string message)
    {
        if(NotificationCoroutine != null)
        {
            StopCoroutine(NotificationCoroutine);
        }
        NotificationCoroutine = FadeNotification(message);
        StartCoroutine(NotificationCoroutine);
    }
    IEnumerator FadeNotification(string message)
    {
        tempMessage = message;
        notificationText.text = message;
        animator.SetTrigger("Fade");
        yield return new WaitForSeconds(resetTextTime);
        notificationText.text = "";
    }
    public void ResetText()
    {
        notificationText.text = string.Empty;
    }
}
