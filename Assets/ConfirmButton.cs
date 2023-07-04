using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class ConfirmButton : MonoBehaviour
{
    public bool canChangeScene = false;
    public CharacterCardBattleController characterCardBattleController;
    public GameObject notification;

    private void Start()
    {
        characterCardBattleController = FindObjectOfType<CharacterCardBattleController>();
    }

    private void Update()
    {
    }

    public void Confirm()
    {
        canChangeScene = true;
        for (int i = 0; i < characterCardBattleController.slotList.Count; i++)
        {
            if (characterCardBattleController.slotList[i] != null)
            {
                CharacterCardDisplay characterCardDisplay
                    = characterCardBattleController.slotList[i].GetComponentInChildren<CharacterCardDisplay>();
                if (characterCardDisplay == null)
                {
                    canChangeScene = false;
                    string text1 = "Need 1 more character card";
                    string text2 = "Need 2 more character cards";
                    string text3 = "Need 3 more character cards";
                    NotificationManager notification = this.notification.GetComponent<NotificationManager>();
                    if (notification.isActiveAndEnabled)
                    {
                        notification.HideObj();
                        notification.ShowObj();
                    }
                    else
                    {
                        notification.ShowObj();
                    }
                    if (characterCardBattleController.slotList[0].GetComponentInChildren<CharacterCardDisplay>() == null)
                        notification.GetTextNotification(text3);
                    else if (characterCardBattleController.slotList[1].GetComponentInChildren<CharacterCardDisplay>() == null)
                        notification.GetTextNotification(text2);
                    else if (characterCardBattleController.slotList[2].GetComponentInChildren<CharacterCardDisplay>() == null)
                        notification.GetTextNotification(text1);
                }
            }
        }
    }
}
