using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GameObject Field", menuName = "ElMasna3/Fields/GameObject Field")]
public class GameObjectField : ScriptableObject
{
    public GameObject gameObjectReference = null; 
}
