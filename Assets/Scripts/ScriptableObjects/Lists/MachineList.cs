using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public List<Machine> GetMachinesByCurrency(Currency currency)
    {
        return Items.FindAll(x=> x.scheme.moneyCurrency == currency);
    }

    public float GetPastCyclesMoneyByCurrency(float t, Currency currency)
    {
        var machines = GetMachinesByCurrency(currency);

        float total = 0;
        for (int i = 0; i < machines.Count; i++)
        {
            if(machines[i].IsWorking)
                total += machines[i].GetMoneyMissedAndSetNew(t);
        }

        return total;
    }

    public int GetNewId()
    {
        return Items.Count + 1;
    }
}
