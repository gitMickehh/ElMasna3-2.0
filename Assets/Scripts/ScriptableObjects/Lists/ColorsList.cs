using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Colors List", menuName = "ElMasna3/Lists/Colors List")]
public class ColorsList : ScriptableObject
{
    public List<Color> colors;

    public int Count
    {
        get { return colors.Count; }
    }
}
