using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyMachinePlace : MonoBehaviour
{
    public EmptyMachinePlaceList emptyMachinesList;

    private void OnEnable()
    {
        emptyMachinesList.Add(this);
    }

    private void OnDisable()
    {
        emptyMachinesList.Remove(this);
    }
}
