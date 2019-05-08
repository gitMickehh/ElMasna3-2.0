using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GuideBookTab
{
    public string name;
    public Sprite TabIcon;
    public GuideBookPage[] listOfPages;
}

[CreateAssetMenu(fileName = "Guide Book", menuName = "ElMasna3/Guide Book/Guide Book")]
public class GuideBook : ScriptableObject
{
    public GuideBookTab[] tabs;
}
