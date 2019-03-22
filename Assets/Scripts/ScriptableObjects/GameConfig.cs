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
    public float happinessPercentage;

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