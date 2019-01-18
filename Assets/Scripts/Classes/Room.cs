using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MachinePosition
{
    public Transform machinePosition;
    public Machine machine;
}

[System.Serializable]
public class Room
{
    public MachinePosition[] machinePlaces;
}
