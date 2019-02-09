using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Machines List", menuName = "ElMasna3/Lists/Machines RT List")]
public class MachineList : RuntimeList<Machine>
{
    public Machine GetMachineByID(int id)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].machineID == id)
                return Items[i];
        }

        return Items[0];
    }

    public int GetNewId()
    {
        return Items.Count + 1;
    }
}
