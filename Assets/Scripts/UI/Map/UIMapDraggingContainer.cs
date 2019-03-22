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
    public FloorList floorList;

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
        SeekRoom workerSeeker = workerComponent.GetComponent<SeekRoom>();
        Floor floor = myFloor.realFloor;

        iconWorker.GiveContainerRef(this);

        if (type == UIDraggingType.MACHINE)
        {
            if(workerComponent.currentMachine == null)
            {
                //the worker was not on a machine and was on a break

                WayPoint oldWayPoint = workerSeeker.wayPointCurrent;
                int breakRoomIndex = floor.GetBreakRoomIndex(waypoint);

                if (breakRoomIndex == -1)
                {
                    //the break room is not on this floor
                    Floor floorWithBreakRoom = floorList.GetFloorWithWayPoint(oldWayPoint);
                    breakRoomIndex = floorWithBreakRoom.GetBreakRoomIndex(oldWayPoint);
                    floorWithBreakRoom.breakRoom[breakRoomIndex].worker = null;
                }
                else
                {
                    floor.breakRoom[breakRoomIndex].worker = null;
                }
            }
            
            machineReference.ChangeWorker(myFloor.listOfWorkers.GetWorkerById(iconWorker.workerID));
        }
        else if (type == UIDraggingType.BREAKWAYPOINT)
        {
            int breakRoomIndex = floor.GetBreakRoomIndex(waypoint);
            Debug.Log("Floor "+floor.floorOrder + " Waypoint Index: "+ breakRoomIndex);

            workerComponent.currentMachine.CurrentWorker = null;
            workerComponent.currentMachine = null;
            floor.breakRoom[breakRoomIndex].worker = workerComponent;
            workerComponent.transform.SetParent(floor.WorkersHolder);
            
            workerSeeker.SwitchRoom(waypoint);
            workerComponent.SetBreak(floor.breakRoom[breakRoomIndex].breakObject);
        }
    }

    //public void Copy(UIMapDraggingContainer other)
    //{
    //    workerImage = other.workerImage;
    //    waypoint
    //}
}
