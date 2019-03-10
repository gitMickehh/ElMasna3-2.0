using UnityEngine;

[System.Serializable]
public class BreakRoomPlace
{
    public WayPoint position;
    public Worker worker;

    public bool IsEmpty {
        get {
            if (worker == null)
                return true;
            else
                return false;
        }
    }
}
