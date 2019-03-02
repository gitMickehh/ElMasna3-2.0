using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Currency
{
    RealMoney,
    HappyMoney
}

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

[CreateAssetMenu(fileName = "New Store Scheme", menuName = "ElMasna3/Store Scheme")]
public class StoreScriptableObject : ScriptableObject
{
    public Sprite RealMoneyIcon;
    public Sprite HappyMoneyIcon;

    [Header("Events")]
    public float PartyCost;

    [Header("Machines")]
    public StoreItem[] Machines;

}
