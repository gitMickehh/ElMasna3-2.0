using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rotators List", menuName = "ElMasna3/Lists/Rotators RT List")]
public class RotatingObjectsList : RuntimeList<ControlledRotator>
{
    public void SetActiveRotator(ControlledRotator rot)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i] == rot)
                rot.currentActive = true;
            else
                Items[i].currentActive = false;
        }
    }

    public ControlledRotator GetActiveRotator()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].currentActive)
            {
                return Items[i];
            }
        }

        return null;
    }
}
