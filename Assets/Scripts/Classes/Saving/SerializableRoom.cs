using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableRoom
{
    public SerializableMachine[] machines;

    public SerializableRoom(SerializableMachine[] machinesHere)
    {
        machines = machinesHere;
    }
}
