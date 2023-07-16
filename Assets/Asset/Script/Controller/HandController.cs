using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandController : DropZone
{
    public GameObject cardToHand;
    public int random;

    public HorizontalLayoutGroup horizontalLayoutGroup;
    private void Start()
    {
        horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
        horizontalLayoutGroup.padding.bottom = 0;
        StartCoroutine(StartGame());
    }
    IEnumerator StartGame()
    {
        for (int i = 0; i < random; i++)
        {
            if (cardToHand.GetComponent<CardDisplay>())
            {
                yield return new WaitForSeconds(0.25f);
                Instantiate(cardToHand, transform.position, transform.rotation);
            }
        }
    }
}
