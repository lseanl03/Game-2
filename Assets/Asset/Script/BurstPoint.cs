using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstPoint : MonoBehaviour
{
    private void OnEnable()
    {
            transform.DOScale(transform.localScale.x + 0.25f, 0.25f).SetLoops(2, LoopType.Yoyo);
    }
}
