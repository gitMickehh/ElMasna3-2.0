using System.Collections;
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

    public int GetNumberOfHiredWorkers()
    {
        return Items.Count(x => !x.inOrientation);
    }

    public int GetNumberOfWorkersInOrientation()
    {
        return Items.Count(x => x.inOrientation);
    }

    public List<Worker> WorkersInOrientation()
    {
        return Items.FindAll(x=>x.inOrientation);
    }

    public void UpdateWorkers()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (!Items[i].inOrientation)
            {
                Items[i].ModifyHappiness();
                Items[i].ModifyHealth();
            }
        }
    }

    public void UpdateWorkersExperience()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (!Items[i].inOrientation && !(Items[i].workerState == WorkerState.InBreak))
                Items[i].ModifyExperience();
        }
    }

    public float CalculateAverageHappiness()
    {
        float happinessSum = 0;
        int workersNo = 0;
        for(int i = 0; i < Items.Count; i++)
        {
            if (!Items[i].inOrientation)
            {
                workersNo++;
                happinessSum += Items[i].happyMeter;
            }
        }
        Debug.Log("happinessSum: " + happinessSum + " workersNo: " + workersNo);
        Debug.Log("(happinessSum / (float)workersNo): " + (happinessSum / (float)workersNo));
        return (happinessSum / (float)workersNo);
    }
}
