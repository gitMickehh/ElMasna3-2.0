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

    [Header("Input")]
    public FloatField swipeMagnitude;
    public float torqueMultiplyer;

    Rigidbody myBody;

    [Header("Rooms")]
    public Room[] workRooms;

    [Header("Workers")]
    public Transform WorkersHolder;

    private void OnEnable()
    {
        listOfFloors.Add(this);
    }

    private void Start()
    {
        myBody = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        listOfFloors.Remove(this);
    }

    public void RotateFloorRight()
    {
        if (!activeFloor)
            return;

        if (myBody.angularVelocity.y > 0)
            myBody.angularVelocity = Vector3.zero;

        myBody.AddTorque(Vector3.down * torqueMultiplyer * swipeMagnitude.GetValue());
    }

    public void RotateFloorLeft()
    {
        if (!activeFloor)
            return;

        if (myBody.angularVelocity.y < 0)
            myBody.angularVelocity = Vector3.zero;

        myBody.AddTorque(Vector3.up * torqueMultiplyer * swipeMagnitude.GetValue());
    }

    public void StopRotation()
    {
        if (!activeFloor)
            return;

        myBody.angularVelocity = Vector3.zero;
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
                    GameObject machPrefab = MachinesPrefabs.GetPrefabByID(machines[j].machineID);
                    
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
                        }
                    }

                    workRooms[i].machinePlaces[j].machine = machineComponent;

                }
            }
        }
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
