using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Empty Machines List", menuName = "ElMasna3/Lists/Empty Machines RT List")]
public class EmptyMachinePlaceList : RuntimeList<EmptyMachinePlace>
{
    public void DestroyAll()
    {
        for (int i = Items.Count-1; i >= 0; i--)
        {
            Destroy(Items[i].gameObject);
        }
    }
}
