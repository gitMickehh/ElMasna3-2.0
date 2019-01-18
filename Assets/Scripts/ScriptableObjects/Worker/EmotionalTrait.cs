using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Emotion", menuName = "ElMasna3/Traits/Emotion")]
public class EmotionalTrait : ScriptableObject {

    public string Title;

    [TextArea]
    public string Description;

    public override string ToString()
    {
        return Title;
    }

    public Sprite emotionIcon;

}
