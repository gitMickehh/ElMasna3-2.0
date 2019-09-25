using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Guide Book Page", menuName = "ElMasna3/Guide Book/Guide Book Page")]
public class GuideBookPage : ScriptableObject
{
    public string PageTitle;

    [TextArea(8,30)]
    public string PageDescription;

    [Header("Foreign Language")]
    [Tooltip("If the text is in a foreign language and you want to use images instead.")]
    public bool ImageDescription;
    public Sprite DescriptionTextImage;

    [Tooltip("All the same size plz. 1920*1080")]
    public Sprite PageImage;

    [Space]
    [Tooltip("Voice Over")]
    public AudioClip VO_Clip;

}
