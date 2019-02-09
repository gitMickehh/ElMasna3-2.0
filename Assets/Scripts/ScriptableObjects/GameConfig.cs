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
    public DateTimeScriptable FirstTimeEver;
    public DateTimeScriptable LastTimePlayed;

    [Header("Building")]
    public float FloorCost;

    //[Header("Player")]
    //public string PlayerName;
    //public int PlayerLevel;
    //public Gender PlayerGender;
    //public GameObject PlayerPrefab;
}