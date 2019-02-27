using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastingByTouch : MonoBehaviour
{
    public float rayLength = 100;

    [Header("Layer Masks")]
    public LayerMask WorkerLayerMask;
    public LayerMask MachineLayerMask;
    public LayerMask VFXEmptyMachineLayerMask;

    public void RaycastingFromScreenPoint()
    {
        if (Input.touchCount == 1)
        {
            //raycast
            int layerMask = LayerMask.GetMask("Worker");

            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (Physics.Raycast(ray, out hit, rayLength, layerMask))
                    Debug.Log(hit.collider.name);

            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            int layerMask = LayerMask.GetMask("Worker");

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, rayLength, layerMask))
                Debug.Log(hit.collider.name);

        }
    }
}
