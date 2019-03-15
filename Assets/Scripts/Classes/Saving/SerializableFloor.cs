using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableFloor
{
    public SerializableRoom[] rooms;
    public int[] breakRoomWorkersIDs;
    public int floorOrder;

    public SerializableFloor(SerializableRoom[] saveRooms, int[] workerIds, int fOrder)
    {
        rooms = saveRooms;
        breakRoomWorkersIDs = workerIds;
        floorOrder = fOrder;
    }
}
