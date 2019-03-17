using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UIDraggingType
{
    MACHINE,
    BREAKWAYPOINT
}

public class UIMapDraggingContainer : MonoBehaviour
{
    public UIDraggingType type;
    [Header("Machine")]
    public Machine machineReference;
    [Header("Waypoint")]
    public WayPoint waypoint;

    [Attributes.GreyOut]
    public GameObject workerImage;
    UIFloor myFloor;

    private void OnEnable()
    {
        myFloor = GetComponentInParent<UIFloor>();
    }

    public void ReleaseDrag(GameObject w)
    {
        var iconWorker = w.GetComponent<WorkerUIIcon>();
        Worker workerComponent = myFloor.listOfWorkers.GetWorkerById(iconWorker.workerID);
        SeekRoom workerSeeker = w.GetComponent<SeekRoom>();
        Floor floor = myFloor.realFloor;

        if (type == UIDraggingType.MACHINE)
        {
            if (iconWorker.machine != null)
            {
                if (workerImage != null)
                {
                    //other machine has worker

                }
                else
                {
                    iconWorker.machine.workerImage = null;
                }
            }

            workerImage = w;
            iconWorker.machine = this;

            machineReference.ChangeWorker(myFloor.listOfWorkers.GetWorkerById(iconWorker.workerID));
        }
        else if (type == UIDraggingType.BREAKWAYPOINT)
        {
            int breakRoomIndex = floor.GetBreakRoomIndex(waypoint);

            floor.breakRoom[breakRoomIndex].worker = workerComponent;
            workerSeeker.SwitchRoom(waypoint);
        }
    }

}
