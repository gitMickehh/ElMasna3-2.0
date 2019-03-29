using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateFactoryInfo : MonoBehaviour
{
    public WorkerList workerList;
    public FloorList floorList;

    public Text workerNoText;
    public Text realMachineNoText;
    public Text happyMachineNoText;
    
    public void UpdateWorkerNo()
    {
        workerNoText.text = workerList.GetNumberOfHiredWorkers().ToString();
    }

    public void UpdateMachinesNo()
    {
        realMachineNoText.text = floorList.noOfRealMachines.ToString();
        happyMachineNoText.text = floorList.noOfHappyMachines.ToString();
    }

}
