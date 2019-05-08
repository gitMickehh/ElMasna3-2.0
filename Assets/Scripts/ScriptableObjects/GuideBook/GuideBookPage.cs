using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Guide Book Page", menuName = "ElMasna3/Guide Book/Guide Book Page")]
public class GuideBookPage : ScriptableObject
{
    public string PageTitle;

    [TextArea(8,30)]
    public string PageDescription;

    [Tooltip("All the same size plz. 1920*1080")]
    public Sprite PageImage;
}
