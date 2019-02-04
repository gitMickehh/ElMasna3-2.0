using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Medical Trait", menuName = "ElMasna3/Traits/Medical Trait")]
public class MedicalTrait : ScriptableObject {

    public string Title;
    public int no;

    [TextArea]
    public string Description;

    public override string ToString()
    {
        return Title;
    }

    public Sprite medicalIcon;
}
