using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableMachine
{
    public bool machineExists;

    public int machineID;
    public int machineModelID;
    public int workerID;

    public SerializableMachine(bool exists = false,int mchId = -1, int modelID = 1, int wId = -1)
    {
        machineExists = exists;
        machineID = mchId;
        machineModelID = modelID;
        workerID = wId;
    }
}
