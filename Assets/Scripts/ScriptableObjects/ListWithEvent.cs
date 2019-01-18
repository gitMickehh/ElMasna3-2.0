using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Evented List", menuName = "ElMasna3/Lists/List With Event")]
public class ListWithEvent : ScriptableObject {

    //[SerializeField]
    public List<ScriptableObject> listElements;

    public GameEvent elementUpdate;
    
    public void AddElement(ScriptableObject objectAdded)
    {
        listElements.Add(objectAdded);
        if(elementUpdate != null)
            elementUpdate.Raise();
    }

    public void RemoveElement(ScriptableObject objectRemoved)
    {
        listElements.Remove(objectRemoved);
        if(elementUpdate != null)
            elementUpdate.Raise();
    }
}
