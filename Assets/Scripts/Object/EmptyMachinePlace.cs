using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyMachinePlace : MonoBehaviour
{
    public EmptyMachinePlaceList emptyMachinesList;
    Floor parentFloor;
    int roomNumber;
    int placeInRoom;

    [Header("Events")]
    public GameEvent placeMachineEvent;

    private void OnEnable()
    {
        emptyMachinesList.Add(this);
    }

    private void OnDisable()
    {
        emptyMachinesList.Remove(this);
    }

    public void SetParentFloor(Floor f, int i, int j)
    {
        parentFloor = f;
        roomNumber = i;
        placeInRoom = j;
    }

    public void PlaceMachine(GameObject go)
    {
        var machineGameObject = Instantiate(go, transform.parent);
        parentFloor.workRooms[roomNumber].machinePlaces[placeInRoom].machine = machineGameObject.GetComponent<Machine>();
        placeMachineEvent.Raise();
        Destroy(gameObject);
    }
}
