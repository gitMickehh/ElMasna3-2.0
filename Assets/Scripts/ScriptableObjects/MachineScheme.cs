using UnityEngine;

[CreateAssetMenu(fileName = "New Machine Scheme", menuName = "ElMasna3/Machine Scheme")]
public class MachineScheme : ScriptableObject
{
    public float moneyInCycle;
    [Range(1,120.0f)]
    public float timeOfCycle;

    public Currency moneyCurrency;
}