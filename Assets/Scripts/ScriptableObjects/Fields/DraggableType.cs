using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Draggable UI", menuName = "ElMasna3/Draggable UI")]
public class DraggableType : ScriptableObject
{
    public bool draggable;
    public bool refreshable;


    public Sprite elementSprite;
}

