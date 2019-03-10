using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [Attributes.GreyOut]
    public int floorOrder;

    [Header("Lists")]
    public GameConfig GameConfigFile;
    public FloorList listOfFloors;
    public WorkerList listOfWorkers;

    [HideInInspector]
    public SerializableFloor savingFloor;

    [Header("Camera Properties")]
    public CameraProperties cameraProperties;
    public float heightToAddToCamera;

    [Header("Rooms")]
    public Room[] workRooms;
    public BreakRoomPlace[] breakRoom;

    [Header("Workers")]
    public Transform WorkersHolder;

    [Header("Machine Mods")]
    public GameObject EmptyMachineVFX;

    private void OnEnable()
    {
        floorOrder = listOfFloors.GetFloorOrder();
        listOfFloors.Add(this);

        cameraProperties.maximumHeight += heightToAddToCamera;
        gameObject.name = "Floor " + floorOrder;
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

        savingFloor = new SerializableFloor(savablerooms, floorOrder);
    }

    public void LoadFloor()
    {
        floorOrder = savingFloor.floorOrder;

        for (int i = 0; i < savingFloor.rooms.Length; i++)
        {
            var machines = savingFloor.rooms[i].machines;

            for (int j = 0; j < machines.Length; j++)
            {
                if (machines[j].machineExists)
                {
                    Transform instPos = workRooms[i].machinePlaces[j].machinePosition;
                    GameObject machPrefab = GameConfigFile.MachinesPrefabs.GetPrefabByID(machines[j].machineModelID);

                    //instantite machines
                    var machCreated = Instantiate(machPrefab, instPos);
                    var machineComponent = machCreated.GetComponent<Machine>();
                    machineComponent.parentFloor = this;

                    if (machines[j].workerID > 0)
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

    public void ShowAvailablePlaces()
    {
        for (int i = 0; i < workRooms.Length; i++)
        {
            for (int j = 0; j < workRooms[i].machinePlaces.Length; j++)
            {
                if (workRooms[i].machinePlaces[j].machine == null)
                {
                    var vfxObj = Instantiate(EmptyMachineVFX, workRooms[i].machinePlaces[j].machinePosition);
                    vfxObj.GetComponent<EmptyMachinePlace>().SetParentFloor(this,i,j);
                }
            }
        }

    }

    public void PlaceMachine(Machine m, int roomNumber, int machinePosition)
    {
        workRooms[roomNumber].machinePlaces[machinePosition].machine = m;
        m.parentFloor = this;

        workRooms[roomNumber].machinePlaces[machinePosition].machinePosition.GetComponent<WayPoint>().WayPointTransform = m.workerPosition;
    }
}
