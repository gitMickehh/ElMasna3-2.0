using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New String List", menuName = "ElMasna3/Fields/List Of Strings")]
public class ListOfStrings : ScriptableObject {

    public List<string> strings;


    public void ClearList()
    {
        strings.Clear();
    }

}
