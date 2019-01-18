using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastingForFloor : MonoBehaviour {

    //public FloorManager floorManager;
    
    //Vector3 target;
    public LayerMask floorLayer;

    public BoolField moving;

    float shootingTime = 0;

    //private void Start()
    //{
    //    floorManager = FindObjectOfType<FloorManager>();
    //}

    private void FixedUpdate()
    {

        if (moving.BoolValue)
        {
            shootingTime += Time.fixedDeltaTime;

            if (shootingTime >= 0.5f)
            {
                moving.BoolValue = false;
                shootingTime = 0;
            }
            else
            {
                ShootRaysFixedUpdate();
            }
        }
    }

    void ShootRaysFixedUpdate()
    {
        var targetS = new Vector3(Screen.width / 2, Screen.height / 2);
        var r = Camera.main.ScreenPointToRay(targetS);

        RaycastHit info;
        Physics.Raycast(r,out info, 150, floorLayer);
        if(info.collider != null)
        {
            //Debug.Log("hit floor :) -> "+ info.collider.gameObject.name);
            //floorManager.UpdateFloorSelected(info.collider.gameObject);
        }

    }

}
