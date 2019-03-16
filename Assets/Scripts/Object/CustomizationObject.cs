using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationObject : MonoBehaviour
{
    public CustomizationItem myself;

    public void loadObject(CustomizationItem c, Transform parent)
    {
        myself.id = c.id;
        myself.happyAdd = c.happyAdd;
        myself.price = c.price;
        myself.type = c.type;

        myself.item = Instantiate(c.item, parent);
        myself.item.layer = 11;
    }
}
