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

}
