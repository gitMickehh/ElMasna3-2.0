using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Float", menuName = "ElMasna3/Fields/Float Field")]
public class FloatField : ScriptableObject{

    [SerializeField]
    float value;

    [Tooltip("You don't have to use it!")]
    public GameEvent onChangeEvent;

    public void SetValue(float v)
    {
        value = v;
        //fire event here
        if (onChangeEvent != null)
            onChangeEvent.Raise();
    }

    public void AddValue(float v)
    {
        value += v;

        if (onChangeEvent != null)
            onChangeEvent.Raise();
    }

    public float GetValue()
    {
        return value;
    }

}
