using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gender
{
    MALE,
    FEMALE
}

[CreateAssetMenu(fileName = "New GameConfig", menuName = "ElMasna3/Game Config")]
public class GameConfig : ScriptableObject {

    [Header("Language")]
    public LanguageProfile CurrentLanguageProfile;

    [Header("Dates")]
    public Day dayOfTheWeek;

    [Header("Workers Data")]
    public float timeToDecreaseHappiness = 1;

    [Header("Wallet")]
    public FloatField FactoryMoney;
    public FloatField HappyMoney;

    [Header("Costs")]
    public float FloorCost;
    public float HiringCost = 100.0f;

    [Header("Prefabs")]
    public GameObject FloorPrefab;
    public PrefabsList WorkersPrefabs;
    public PrefabsList MachinesPrefabs;

    [Header("Player Data")]
    public ColorField uniformColor;
    [SerializeField]
    private Material uniformMaterial;

    [Header("Modal Panel Icons")]
    public Sprite[] icons;

    [Header("Party")]
    public float PartyCost = 500;
    public float happinessPercentage;

    [Header("Customization Items Tiers")]
    public int tier1;
    public int tier2;
    public int tier3;

    public CustomizationTier GetTier(int level)
    {
        CustomizationTier tier;

        if(level >= tier3)
        {
            tier = CustomizationTier.TIER3;
        }
        else if(level >= tier2)
        {
            tier = CustomizationTier.TIER2;
        }
        else
        {
            tier = CustomizationTier.TIER1;
        }

        return tier;
    }

    public void SetColorField(Color c)
    {
        uniformColor.SetValue(c);
        uniformMaterial.color = c;
    }

    //[Header("Player")]
    //public string PlayerName;
    //public int PlayerLevel;
    //public Gender PlayerGender;
    //public GameObject PlayerPrefab;
}