using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Medical Trait", menuName = "ElMasna3/Traits/Medical Trait")]
public class MedicalTrait : ScriptableObject {

    public string Title;
    public int no;
    [Range(0,1)]
    public float rate;
    public float start;

    [TextArea]
    public string Description;

    public override string ToString()
    {
        return Title;
    }

    public Sprite medicalIcon;
}
