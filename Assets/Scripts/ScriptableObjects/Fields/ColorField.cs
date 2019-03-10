using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Color", menuName = "ElMasna3/Fields/Color Field")]
public class ColorField : ScriptableObject {

    [SerializeField]
    Color value = Color.red;

    [Tooltip("You don't have to use it!")]
    public GameEvent onChangeEvent;

    public void SetValue(Color v)
    {
        value = v;
        //fire event here
        if (onChangeEvent != null)
            onChangeEvent.Raise();
    }

    public Color GetValue()
    {
        return value;
    }

}
