using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Workers Icons List", menuName = "ElMasna3/UI/Workers Icons RT List")]
public class WorkerUIIconList : RuntimeList<WorkerUIIcon>
{
    public WorkerUIIcon GetWorkerIconByID(int id)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].workerID == id)
                return Items[i];
        }

        return null;
    }

    public void DisableRaycastExcept(int id)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if(Items[i].workerID != id)
            {
                Items[i].gameObject.GetComponent<Image>().raycastTarget = false;
            }
        }
    }

    public void EnableRaycastTargets()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            Items[i].gameObject.GetComponent<Image>().raycastTarget = true;
        }
    }
}
