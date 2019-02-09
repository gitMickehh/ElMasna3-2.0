using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameConfig GameConfigFile;

    [Header("Factory")]
    public FloatField FactoryMoney;
    public FloatField HappyMoney;

    [Header("Building Floors")]
    public GameObject Floor;
    public FloorList listOfFloors;
    public GameEvent BuildSuccess;
    public GameEvent BuildFailure;

    public void BuildFloor()
    {
        if(FactoryMoney.GetValue() >= GameConfigFile.FloorCost)
        {
            FactoryMoney.AddValue(-GameConfigFile.FloorCost);

            var f = Instantiate(Floor, new Vector3(), new Quaternion());

            float heightOfFloor = f.GetComponentInChildren<Collider>().bounds.max.y;
            Vector3 position = Vector3.up * heightOfFloor * (f.GetComponent<Floor>().floorOrder - 1);
            f.transform.position = position;

            BuildSuccess.Raise();
        }
        else
        {
            BuildFailure.Raise();
        }
        
    }
}
