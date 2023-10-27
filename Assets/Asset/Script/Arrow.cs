using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArrowType
{
    top,
    bottom,
}
public class Arrow : MonoBehaviour
{
    public ArrowType arrowType;
    private Tween moveYTween;
    public void Start()
    {
        StartCoroutine(Animation());
    }
    IEnumerator Animation()
    {
        if (gameObject.activeSelf)
        {
            moveYTween = transform.DOLocalMoveY(transform.localPosition.y + 15f, 0.5f).SetEase(Ease.InSine).SetLoops(-1, LoopType.Yoyo);
            yield return null;
        }
    }
    private void OnDestroy()
    {
        if(moveYTween != null) moveYTween.Kill();
    }
}
