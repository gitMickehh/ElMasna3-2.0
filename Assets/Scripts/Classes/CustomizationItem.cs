using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CustomizationType
{
    HEAD,
    FACE,
    BODY
}

[System.Serializable]
public class CustomizationItem
{
    public int id;

    [Range(1,100)]
    public float happyAdd = 50;

    public GameObject item;
    public CustomizationType type;
    public float price;
}
