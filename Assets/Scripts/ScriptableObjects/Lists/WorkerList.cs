using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Workers List", menuName = "ElMasna3/Lists/Workers RT List")]
public class WorkerList : RuntimeList<Worker>
{

    public Worker GetWorkerById(int id)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].ID == id)
                return Items[i];
        }

        Debug.LogError("Worker id ("+ id +") does not exist!");
        return null;
    }

    public int GetNewId()
    {
        return Items.Count + 1;
    }
}
