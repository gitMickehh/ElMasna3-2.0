using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Vector2", menuName = "ElMasna3/Fields/Vector2 Field")]
public class Vector2Field : ScriptableObject
{
    public Vector2 vector2;

    public void SetVector2(Vector2 t)
    {
        vector2.x = t.x;
        vector2.y = t.y;
    }
}
