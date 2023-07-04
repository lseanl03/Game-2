using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Information", menuName ="TCG/Information")]
public class InformationData : ScriptableObject
{
    public List<InformationBase> informationBase;
}

[Serializable]
public class InformationBase
{
    public InformationHeader informationHeader;
    [TextArea]public string contentText;
    public Sprite cardSprite;
}
