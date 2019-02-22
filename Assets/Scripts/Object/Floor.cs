using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    //true for testing now
    public bool activeFloor = true;

    public int floorOrder;

    [Header("Lists")]
    public FloorList listOfFloors;
    public WorkerList listOfWorkers;
    public PrefabsList MachinesPrefabs;

    //for saving
    [HideInInspector]
    public SerializableFloor savingFloor;

    [Header("Camera Properties")]
    public CameraProperties cameraProperties;
    public float heightToAddToCamera;

    Rigidbody myBody;

    [Header("Rooms")]
    public Room[] workRooms;

    [Header("Workers")]
    public Transform WorkersHolder;

    private void OnEnable()
    {
        floorOrder = listOfFloors.GetFloorOrder();
        listOfFloors.Add(this);

        cameraProperties.maximumHeight += heightToAddToCamera;
    }

    private void Start()
    {
        myBody = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        listOfFloors.Remove(this);
        cameraProperties.maximumHeight -= heightToAddToCamera;
    }

    public void FillSerializableFloor()
    {
        SerializableRoom[] savablerooms = new SerializableRoom[workRooms.Length];
        for (int i = 0; i < workRooms.Length; i++)
        {
            savablerooms[i] = workRooms[i].SaveRoom();
        }

        savingFloor = new SerializableFloor(savablerooms,floorOrder);
    }

    public void LoadFloor()
    {
        floorOrder = savingFloor.floorOrder;

        for (int i = 0; i < savingFloor.rooms.Length; i++)
        {
            var machines = savingFloor.rooms[i].machines;

            for (int j = 0; j < machines.Length; j++)
            {
                if(machines[j].machineExists)
                {
                    Transform instPos = workRooms[i].machinePlaces[j].machinePosition;
                    GameObject machPrefab = MachinesPrefabs.GetPrefabByID(machines[j].machineModelID);
                    
                    //instantite machines
                    var machCreated = Instantiate(machPrefab,instPos);
                    var machineComponent = machCreated.GetComponent<Machine>();

                    if(machines[j].workerID > 0)
                    {
                        GameObject w = listOfWorkers.GetWorkerById(machines[j].workerID).gameObject;
                        w.transform.SetParent(WorkersHolder);

                        if (w != null)
                        {
                            w.transform.position = machineComponent.workerPosition.position;
                            w.transform.rotation = machineComponent.workerPosition.rotation;

                            machineComponent.CurrentWorker = w;
                            w.GetComponent<SeekRoom>().wayPointCurrent = workRooms[i].machinePlaces[j].machinePosition.GetComponent<WayPoint>();
                            w.GetComponent<Worker>().currentMachine = machineComponent;
                        }
                    }

                    workRooms[i].machinePlaces[j].machine = machineComponent;
                    workRooms[i].machinePlaces[j].machinePosition.GetComponent<WayPoint>().WayPointTransform = 
                        workRooms[i].machinePlaces[j].machine.workerPosition;
                }
            }
        }

        transform.name = "Floor " + floorOrder;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        for (int i = 0; i < workRooms.Length; i++)
        {
            for (int j = 0; j < workRooms[i].machinePlaces.Length; j++)
            {
                Gizmos.DrawWireSphere(workRooms[i].machinePlaces[j].machinePosition.position, 1f);
            }
        }
    }

    public int Compare(Floor x, Floor y)
    {
        if (x.floorOrder > y.floorOrder)
            return 1;
        else if (x.floorOrder == y.floorOrder)
            return 0;
        else
            return -1;
    }
}
