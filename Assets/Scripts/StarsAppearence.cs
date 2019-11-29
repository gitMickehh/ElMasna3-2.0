using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsAppearence : MonoBehaviour
{
    public GameObject stars;
    public List<Transform> starsLocation;
    public IntField starsNo;

    public void ShowingStars()
    {
        //Debug.Log("starsNo.GetValue(): " + starsNo.GetValue());

        for (int i = 0; i < starsNo.GetValue(); i++)
        {
            //Debug.Log("starsNo.GetValue(): " + starsNo.GetValue());
            GameObject starsCopy = Instantiate(stars, starsLocation[i]);
        }
    }
}
