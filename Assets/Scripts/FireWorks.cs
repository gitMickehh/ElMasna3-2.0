using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorks : MonoBehaviour
{
    public GameObject fireWorks;
    public List<Transform> fireWorksLocations;

   public void PartyStarted()
    {
        for(int i = 0; i< fireWorksLocations.Count; i++)
        {
            GameObject fireWorksCopy = Instantiate(fireWorks, fireWorksLocations[i]);

        }
    }
}
