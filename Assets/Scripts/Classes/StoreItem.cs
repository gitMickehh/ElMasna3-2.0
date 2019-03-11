using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoreItem
{
    public string name;
    public GameObject Prefab;
    public Sprite storeIcon;
    public Currency Currency;
    public float price;

    [TextArea]
    public string description;
}
