using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMap : MonoBehaviour
{
    public GameEvent UIOn;
    public GameEvent UIOff;

    public FloorList listOfFloors;


    private void OnEnable()
    {
        UIOn.Raise();
    }

    private void OnDisable()
    {
        UIOff.Raise();
    }

    private void Start()
    {
        FillInMap();
    }

    public void GetFirstFloor()
    {
    }

    public void FillInMap()
    {
        GetFirstFloor();
    }
}
