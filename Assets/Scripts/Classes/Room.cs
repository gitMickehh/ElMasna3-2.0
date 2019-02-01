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

    public SerializableRoom SaveRoom()
    {
        SerializableMachine[] mm = 
            new SerializableMachine[machinePlaces.Length];

        for (int i = 0; i < machinePlaces.Length; i++)
        {
            if(machinePlaces[i].machine != null)
            {
                mm[i] = machinePlaces[i].machine.GetSerializableMachine();
            }
            else
            {
                mm[i] = new SerializableMachine();
            }
        }

        return new SerializableRoom(mm);
    }
}
