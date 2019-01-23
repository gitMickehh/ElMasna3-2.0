using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlingGameState : MonoBehaviour
{
    void Start()
    {
        SaveGameManager.Instance.Load();
    }

    void OnApplicationQuit()
    {
        SaveGameManager.Instance.Save();
        Debug.Log("Application ending after " + Time.time + " seconds");
    }
}
