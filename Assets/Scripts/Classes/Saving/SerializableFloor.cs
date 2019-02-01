using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableFloor
{
    public SerializableRoom[] rooms;

    public SerializableFloor(SerializableRoom[] saveRooms)
    {
        rooms = saveRooms;
    }
}
