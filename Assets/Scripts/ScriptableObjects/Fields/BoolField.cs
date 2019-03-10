using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bool Field", menuName = "ElMasna3/Fields/Bool Field")]
public class BoolField : ScriptableObject
{
    public GameEvent onTrue;
    public GameEvent onFalse;

    [SerializeField]
    private bool boolvalue = false;

    public bool BoolValue {
        get
        {
            return boolvalue;
        }
        set
        {
            boolvalue = value;
            if (value == true)
            {
                if(onTrue != null)
                    onTrue.Raise();
            }
            else
            {
                if (onFalse != null)
                    onFalse.Raise();
            }
        }
    }
}
