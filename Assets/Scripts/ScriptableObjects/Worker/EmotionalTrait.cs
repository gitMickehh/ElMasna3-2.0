using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Emotion", menuName = "ElMasna3/Traits/Emotion")]
public class EmotionalTrait : ScriptableObject {

    public string Title;
    public int no;///////

    [TextArea]
    public string Description;

    public override string ToString()
    {
        return Title;
    }

    public Sprite emotionIcon;

}
