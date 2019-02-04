using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Emotion", menuName = "ElMasna3/Traits/Emotion")]
public class EmotionalTrait : ScriptableObject, IComparer<EmotionalTrait> {

    public string Title;
    public int no;

    [TextArea]
    public string Description;

    public override string ToString()
    {
        return Title;
    }

    public int Compare(EmotionalTrait x, EmotionalTrait y)
    {
        if (x.no > y.no)
            return 1;
        else if (x.no < y.no)
            return -1;
        else
            return 0;
    }

    public Sprite emotionIcon;

}
