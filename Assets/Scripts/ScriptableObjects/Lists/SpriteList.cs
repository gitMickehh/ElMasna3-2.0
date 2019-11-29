using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sprites List", menuName = "ElMasna3/Lists/Sprites List")]
public class SpriteList : ScriptableObject
{
    public List<Sprite> sprites;


    //returns a sprite by rounding up the value
    public Sprite GetSprite(float value)
    {
        //Debug.Log("index is: " + GetIndex(value) + ", value is: " + value);
        return sprites[GetIndex(value)];
    }

    private int GetIndex(float value)
    {
        float index = value * (sprites.Count);
        return Mathf.FloorToInt(index) - 1;
    }

}
