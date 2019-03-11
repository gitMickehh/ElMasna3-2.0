using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Customization Panel Scheme", menuName = "ElMasna3/UI/Customization Panel Scheme")]
public class CustomizationPanelScheme : ScriptableObject
{
    public List<CustomizationItem> Items;
}
