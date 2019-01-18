using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Event", menuName = "ElMasna3/Game Event")]
public class GameEvent : ScriptableObject {
    
   public List<GameEventListener> listeners = new List<GameEventListener>(); //remove public 
	
    public void Raise()
    {
        for (int i = listeners.Count-1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
        else
            Debug.Log("Listener is already registered");
    }

    public void UnregisterListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }

    public void Debugman()
    {
        Debug.Log("yo");
    }

    public void ClearListeners()
    {
        listeners.Clear();
    }

}
