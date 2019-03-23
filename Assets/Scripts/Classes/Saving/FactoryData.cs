using UnityEngine;

[System.Serializable]
public class FactoryData
{
    //Maybe load this in the home screen rather than insinde the game. To get the language.

    public float realMoney;
    public float happyMoney;
    public float partyCost;
    public float workerHireCost;
    public float floorBuildCost;
    public Color uniformColor;

    public int languageProfile;

    public FactoryData(float realMon = 250, float happyMon = 10, float party = 500, float workerHire = 100, float floorBuilding = 300)
    {
        realMoney = realMon;
        happyMoney = happyMon;
        partyCost = party;
        workerHireCost = workerHire;
        floorBuildCost = floorBuilding;

        uniformColor = Color.blue;
    }
}
