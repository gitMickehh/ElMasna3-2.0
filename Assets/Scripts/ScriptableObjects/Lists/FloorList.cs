using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New Floor List", menuName = "ElMasna3/Lists/Floors RT List")]
public class FloorList : RuntimeList<Floor>
{
    [Header("Floor List Events")]
    public GameEvent AddFloorEvent;
    public GameEvent RemoveFloorEvent;

    public int noOfRealMachines;
    public int noOfHappyMachines;

    public override void Add(Floor t)
    {
        base.Add(t);
        AddFloorEvent.Raise();
    }

    public override void Remove(Floor t)
    {
        base.Remove(t);
        RemoveFloorEvent.Raise();
    }

    public void SortFloorList()
    {
        Items.Sort(delegate(Floor x, Floor y) {
             return x.Compare(x, y);
        });
    }

    public int GetFloorOrder()
    {
        return Items.Count + 1;
    }

    public void ActivateAvailableMachinePlaces()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            Items[i].ShowAvailablePlaces();
        }
    }
    
    public Floor GetFloorWithWayPoint(WayPoint wp)
    {
        return Items.First<Floor>(x => x.GetBreakRoomIndex(wp) != -1);
    }

    public Machine GetFirstAvailableMachine()
    {
        SortFloorList();

        for (int i = 0; i < Items.Count; i++)
        {
            for (int j = 0; j < Items[i].workRooms.Length; j++)
            {
                for (int k = 0; k < Items[i].workRooms[j].machinePlaces.Length; k++)
                {
                    if(Items[i].workRooms[j].machinePlaces[k].machine != null)
                    {
                        if(Items[i].workRooms[j].machinePlaces[k].machine.CurrentWorker == null)
                            return Items[i].workRooms[j].machinePlaces[k].machine;
                    }
                }
            }
        }

        return null;
    }

    public WayPoint GetFirstAvailableBreakSpace()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            for (int j = 0; j < Items[i].breakRoom.Length; j++)
            {
                if(Items[i].breakRoom[j].IsEmpty)
                {
                    return Items[i].breakRoom[j].position;
                }
            }
        }

        //the factory has no space left
        return null;
    }

    /// <summary>
    /// checks if there is a room for a new worker.
    /// </summary>
    /// <returns></returns>
    public bool CheckAvailability()
    {
        Machine m = GetFirstAvailableMachine();

        if (m == null)
        {
            WayPoint w = GetFirstAvailableBreakSpace();

            if (w == null)
                return false;
        }

        return true;
    }

    public void UpdateNoOfMachines()
    {
        noOfRealMachines = 0;
        noOfHappyMachines = 0;

        for (int i = 0; i < Items.Count; i++)
        {
            for (int j = 0; j < Items[i].workRooms.Length; j++)
            {
                for (int k = 0; k < Items[i].workRooms[j].machinePlaces.Length; k++)
                {
                    if (Items[i].workRooms[j].machinePlaces[k].machine != null)
                    {
                        if (Items[i].workRooms[j].machinePlaces[k].machine.scheme.moneyCurrency == Currency.RealMoney)
                            noOfRealMachines++;
                        else if (Items[i].workRooms[j].machinePlaces[k].machine.scheme.moneyCurrency == Currency.HappyMoney)
                            noOfHappyMachines++;
                    }
                }
            }
        }
    }
}
