using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPoint : MonoBehaviour
{
    public bool scaleAction = true;
    public void Start()
    {
    }
    private void OnEnable()
    {
        if(scaleAction)
        transform.DOScale(transform.localScale.x + 0.25f, 0.25f).SetLoops(2, LoopType.Yoyo);
    }
    private void OnDisable()
    {
        scaleAction = false;
    }
}
