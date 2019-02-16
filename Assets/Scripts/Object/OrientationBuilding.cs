using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OrientationPosition
{
    public Transform position;
    public Worker worker;
}

public class OrientationBuilding : MonoBehaviour
{
    [Header("Camera")]
    public Transform RoomCamera;

    [Header("Worker Positions")]
    public OrientationPosition[] WorkersPositions;

}
