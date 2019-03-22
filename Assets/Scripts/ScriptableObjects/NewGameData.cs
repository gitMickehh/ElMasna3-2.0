using UnityEngine;

[CreateAssetMenu(fileName = "New Game Data", menuName = "ElMasna3/New Game")]
public class NewGameData : ScriptableObject
{
    [Header("Currency")]
    public float RealMoney = 250;
    public float HappyMoney = 10;

    [Header("Game Start Data")]
    public float FloorCost = 300;
    public float PartyCost = 500;
    public float WorkerHireCost = 100;
    public Color basicUniformColor = Color.green;
}
