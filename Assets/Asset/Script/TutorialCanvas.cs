using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialCanvas : CanvasBase
{
    public bool canSkip = true;
    public bool skipTutorial = false;
    public bool isShowedCharacterTutorial = false;
    public bool isShowedActionCardTutorial = false;
    public bool isShowedAttackTutorial = false;
    public bool isShowedSwitchCharacterTutorial = false;
    public bool isShowedWeaknessTutorial = false;
    public bool isShowedEndRoundTutorial = false;
    public float count = 0;
    public float timer = 1f;
    public TutorialType currentTutorialType;

    public GameObject arrowHolder;
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialText;

    [Header("Prefab")]
    public GameObject arrowTopPrefab;
    private GameObject arrowTop;
    public GameObject arrowBottomPrefab;
    private GameObject arrowBottom;

    [Header("Point")]
    public Transform uiTutorialPoint;
    public Transform characterPoint;
    public Transform actionCardPoint;
    public Transform attackPoint;
    public Transform switchCharacterPoint;
    public Transform weaknessPoint;

    [Header("Data")]
    public TutorialData tutorialData;
    public void Start()
    {
        TutorialPanelState(false);
    }
    public void Update()
    {
        if (skipTutorial)
        {
            ResetIsUsed(true);
        }
        if (!canSkip)
        {
            count += Time.deltaTime;
            if (count >= timer) 
                canSkip = true;
        }

    }
    public void ResetIsUsed(bool state)
    {
        foreach (var tutorialData in tutorialData.tutorialList)
        {
            foreach (TutorialText tutorialText in tutorialData.textList)
            {
                tutorialText.isUsed = state;
            }
        }
        isShowedCharacterTutorial = state;
        isShowedActionCardTutorial = state;
        isShowedAttackTutorial = state;
        isShowedSwitchCharacterTutorial = state;
        isShowedWeaknessTutorial = state;
        isShowedEndRoundTutorial = state;
}
    public void TutorialPanelState(bool state)
    {
        if (!state)
        {
            tutorialText.text = string.Empty;
        }
        tutorialPanel.SetActive(state);
    }
    public void CheckTutorialText()
    {
        bool allTextUsed = true;

        foreach (var tutorialData in tutorialData.tutorialList)
        {
            if (currentTutorialType == tutorialData.tutorialType)
            {
                foreach(TutorialText tutorialText in tutorialData.textList)
                {
                    if (!tutorialText.isUsed && canSkip)
                    {
                        if (arrowBottom != null) Destroy(arrowBottom);
                        if (arrowTop != null) Destroy(arrowTop);

                        if(currentTutorialType == TutorialType.uiTutorial)
                        {
                            if (tutorialText.arrowTop) SpawnArrow(uiTutorialPoint, true, tutorialData.textList.IndexOf(tutorialText));
                            else if (tutorialText.arrowBottom) SpawnArrow(uiTutorialPoint, false, tutorialData.textList.IndexOf(tutorialText));
                        }
                        else if (currentTutorialType == TutorialType.CharacterTutorial)
                        {
                            if (tutorialText.arrowTop) SpawnArrow(characterPoint, true, tutorialData.textList.IndexOf(tutorialText));
                            else if (tutorialText.arrowBottom) SpawnArrow(characterPoint, false, tutorialData.textList.IndexOf(tutorialText));
                        }
                        else if(currentTutorialType == TutorialType.ActionCardTutorial)
                        {
                            isShowedActionCardTutorial = true;
                            if (tutorialText.arrowTop) SpawnArrow(actionCardPoint, true, tutorialData.textList.IndexOf(tutorialText));
                            else if (tutorialText.arrowBottom) SpawnArrow(actionCardPoint, false, tutorialData.textList.IndexOf(tutorialText));
                        }
                        else if (currentTutorialType == TutorialType.AttackTutorial)
                        {
                            if (tutorialText.arrowTop) SpawnArrow(attackPoint, true, tutorialData.textList.IndexOf(tutorialText));
                            else if (tutorialText.arrowBottom) SpawnArrow(attackPoint, false, tutorialData.textList.IndexOf(tutorialText));
                        }
                        else if (currentTutorialType == TutorialType.SwitchCharacterTutorial)
                        {
                            if (tutorialText.arrowTop) SpawnArrow(switchCharacterPoint, true, tutorialData.textList.IndexOf(tutorialText));
                            else if (tutorialText.arrowBottom) SpawnArrow(switchCharacterPoint, false, tutorialData.textList.IndexOf(tutorialText));
                        }
                        else if (currentTutorialType == TutorialType.WeaknessTutorial)
                        {
                            isShowedWeaknessTutorial = true;
                            if (tutorialText.arrowTop) SpawnArrow(weaknessPoint, true, tutorialData.textList.IndexOf(tutorialText));
                            else if (tutorialText.arrowBottom) SpawnArrow(weaknessPoint, false, tutorialData.textList.IndexOf(tutorialText));
                        }
                        allTextUsed = false;
                        tutorialText.isUsed = true;
                        SetTutorialText(tutorialText.text);
                        count = 0;
                        canSkip = false;
                        break;
                    }
                }
            }
        }
        if (allTextUsed && canSkip)
        {
            TutorialPanelState(false);
        }
    }
    public void SetTutorialText(string text)
    {
        tutorialText.text = text;
    }
    public void ActionTutorial(TutorialType tutorialType)
    {
        currentTutorialType = tutorialType;
        TutorialPanelState(true);
        CheckTutorialText();
    }
    public void SpawnArrow(Transform transform, bool haveArrowTop, int index)
    {
        if (haveArrowTop)
        {
            arrowTop = Instantiate(arrowTopPrefab, arrowHolder.transform);
            arrowTop.transform.localPosition = transform.GetChild(index).transform.localPosition;
        }
        else
        {
            arrowBottom = Instantiate(arrowBottomPrefab, arrowHolder.transform);
            arrowBottom.transform.localPosition = transform.GetChild(index).transform.localPosition;
        }
    }
}
