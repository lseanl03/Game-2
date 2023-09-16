using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public Transform card1;
    public GameObject card2;
    public Vector3 vector3;
    public float rotationDuration;
    public float shakeDuration;
    public float shakeStrength;
    public int vibrato;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RotateToCard1();
        }
    }
    private void RotateToCard1()
    {
        card1.DOMove(card2.transform.position, rotationDuration).SetEase(Ease.InBack).SetLoops(2, LoopType.Yoyo);
        card1.DORotate(vector3, rotationDuration).SetLoops(2, LoopType.Yoyo);
        card2.transform.DOShakeRotation(shakeDuration, shakeStrength, vibrato,0,true).SetDelay(rotationDuration);
        card2.transform.DOShakePosition(shakeDuration, shakeStrength, vibrato,0,true).SetDelay(rotationDuration);

    }
    private void Domove1()
    {

    }
    public void ShakeCard2()
    {

    }
}    
