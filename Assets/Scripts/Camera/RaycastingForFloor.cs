using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastingForFloor : MonoBehaviour
{

    public float rayLength;
    public BoolField moving;
    public RotatingObjectsList listOfRotators;

    private void OnEnable()
    {
        moving.BoolValue = false;
    }

    private void FixedUpdate()
    {
        if (moving.BoolValue)
        {
            ShootRaysFixedUpdate();
        }
    }

    void ShootRaysFixedUpdate()
    {
        var targetS = new Vector3(Screen.width / 2, Screen.height / 2);
        var r = Camera.main.ScreenPointToRay(targetS);

        r.direction = transform.forward * rayLength;

        RaycastHit info;
        Physics.Raycast(r, out info);
        if (info.collider != null)
        {
            //Debug.Log(info.collider.gameObject.name);
            listOfRotators.SetActiveRotator(info.collider.gameObject.GetComponent<ControlledRotator>());
        }

    }

}
