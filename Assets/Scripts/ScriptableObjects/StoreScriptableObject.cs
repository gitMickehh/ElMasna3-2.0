using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Currency
{
    RealMoney,
    HappyMoney
}

[CreateAssetMenu(fileName = "New Store Scheme", menuName = "ElMasna3/UI/Store Scheme")]
public class StoreScriptableObject : ScriptableObject
{
    public Sprite RealMoneyIcon;
    public Sprite HappyMoneyIcon;

    [Header("Events")]
    public float PartyCost;

    [Header("Machines")]
    public StoreItem[] Machines;

}
