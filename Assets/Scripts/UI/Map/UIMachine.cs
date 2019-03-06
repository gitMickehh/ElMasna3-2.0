using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMachine : MonoBehaviour
{
    public Machine machineReference;
    public GameObject workerImage;

    UIFloor myFloor;

    private void OnEnable()
    {
        myFloor = GetComponentInParent<UIFloor>();
    }

    public void ChangeWorker(GameObject w)
    {
        if (w.GetComponent<WorkerUIIcon>().machine != null)
            w.GetComponent<WorkerUIIcon>().machine.workerImage = null;

        workerImage = w;
        w.GetComponent<WorkerUIIcon>().machine = this;

        machineReference.ChangeWorker(myFloor.listOfWorkers.GetWorkerById(workerImage.GetComponent<WorkerUIIcon>().workerID));
    }

}
