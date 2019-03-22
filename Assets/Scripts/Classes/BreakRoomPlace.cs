using UnityEngine;

public enum BreakObject
{
    Sit,
    Talk,
    Stand
}

[System.Serializable]
public class BreakRoomPlace
{
    public WayPoint position;
    public Worker worker;
    public BreakObject breakObject;

    public bool IsEmpty {
        get {
            if (worker == null)
                return true;
            else
                return false;
        }
    }
    
    

}
