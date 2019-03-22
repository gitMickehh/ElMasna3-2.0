﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        Items = Items.OrderBy(x => x.ID).ToList();

        if (Items.Count == 0)
            return Items.Count + 1;
        else
            return Items.Last().ID + 1;
    }

    public void AddPartyHappiness(float happinessPercentage)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (!Items[i].inOrientation)
            {
                Items[i].AddHappiness(happinessPercentage);
                Items[i].workerAnimator.SetTrigger("Winning");
            }
        }
    }
}
