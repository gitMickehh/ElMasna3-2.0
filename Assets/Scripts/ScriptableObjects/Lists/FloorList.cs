using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New Floor List", menuName = "ElMasna3/Lists/Floor List")]
public class FloorList : RuntimeList<Floor>
{
    public void SortFloorList()
    {
        Items.Sort(delegate(Floor x, Floor y) {
             return x.Compare(x, y);
        });
    }
}
