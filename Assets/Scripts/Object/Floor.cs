using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    //public int ID;

    //true for testing now
    public bool activeFloor = true;

    [Header("Input")]
    public FloatField swipeMagnitude;
    public float torqueMultiplyer;

    Rigidbody myBody;

    public Room[] workRooms;

    public int noOfMachines;

    private void Start()
    {
        myBody = GetComponent<Rigidbody>();
    }

    public void RotateFloorRight()
    {
        if (myBody.angularVelocity.y > 0)
            myBody.angularVelocity = Vector3.zero;

        myBody.AddTorque(Vector3.down * torqueMultiplyer * swipeMagnitude.GetValue());
    }

    public void RotateFloorLeft()
    {
        if (myBody.angularVelocity.y < 0)
            myBody.angularVelocity = Vector3.zero;    

        myBody.AddTorque(Vector3.up * torqueMultiplyer * swipeMagnitude.GetValue());
    }

    public void StopRotation()
    {
        myBody.angularVelocity = Vector3.zero;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.white;

    //    for (int i = 0; i < workRooms.Length; i++)
    //    {
    //        for (int j = 0; j < workRooms[i].machinePlaces.Length ; j++)
    //        {
    //            Gizmos.DrawWireSphere(workRooms[i].machinePlaces[j].machinePosition.position,1f);
    //        }
    //    }
    //}

    //public void SaveFloor()
    //{
    //    SaveSystem.SaveFloor(this);
    //}

    //public void LoadFloor()
    //{
    //    FloorData data = SaveSystem.LoadFloor();

    //    noOfMachines = data.noOfMachines;

    //}
}
