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
    public WorkerUIIcon myWorkerImage
    {
        get
        {
            return workerImage.GetComponent<WorkerUIIcon>();
        }
    }
    UIFloor myFloor;

    private void Start()
    {
        myFloor = GetComponentInParent<UIFloor>();
    }

    public void ReleaseDrag(GameObject workerUIGameObject)
    {
        var iconWorker = workerUIGameObject.GetComponent<WorkerUIIcon>();
        Worker workerComponent = myFloor.listOfWorkers.GetWorkerById(iconWorker.workerID);
        SeekRoom workerSeeker = workerUIGameObject.GetComponent<SeekRoom>();
        Floor floor = myFloor.realFloor;

        if (type == UIDraggingType.MACHINE)
        {
            if (iconWorker.container != null)
            {
                if (workerImage != null)
                {
                    //if worker was on another machine and I had a worker
                    //then swap workers
                    WorkerUIIcon temp = iconWorker.container.myWorkerImage;
                    iconWorker.container.workerImage = workerImage;
                    
                }
                else
                {
                    //if worker is already on another machine and I have no workers
                    iconWorker.container.workerImage = null;
                    workerImage = workerUIGameObject;
                }
            }

            iconWorker.container = this;

            machineReference.ChangeWorker(myFloor.listOfWorkers.GetWorkerById(iconWorker.workerID));
        }
        else if (type == UIDraggingType.BREAKWAYPOINT)
        {
            int breakRoomIndex = floor.GetBreakRoomIndex(waypoint);


            floor.breakRoom[breakRoomIndex].worker = workerComponent;
            workerSeeker.SwitchRoom(waypoint);
        }
    }

    //public void Copy(UIMapDraggingContainer other)
    //{
    //    workerImage = other.workerImage;
    //    waypoint
    //}
}
