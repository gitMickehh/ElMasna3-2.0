using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Draggable UI", menuName = "ElMasna3/UI/Draggable UI")]
public class DraggableType : ScriptableObject
{
    public bool draggable = false;
    public bool refreshable = false;


    public Sprite elementSprite;
}

