using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableFloor
{
    public SerializableRoom[] rooms;
    public int floorOrder;

    public SerializableFloor(SerializableRoom[] saveRooms, int fOrder)
    {
        rooms = saveRooms;
        floorOrder = fOrder;
    }
}
