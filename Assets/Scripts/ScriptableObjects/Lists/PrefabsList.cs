using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabReference
{
    public int referenceID = 0;
    public GameObject prefab = null;
}

[CreateAssetMenu(fileName = "New Prefabs list", menuName = "ElMasna3/Lists/Prefabs")]
public class PrefabsList : ScriptableObject
{
    public PrefabReference[] prefabs;

    public GameObject GetPrefabByID(int id)
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i].referenceID == id)
                return prefabs[i].prefab;
        }

        return null;
    }
}
