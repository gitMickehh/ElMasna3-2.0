using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastingByTouch : MonoBehaviour
{
    public float rayLength = 100;
    //public bool touch = true;
    public Vector2Field vector2Touch;

    [Header("Layer Masks")]
    public LayerMask WorkerLayerMask;
    public LayerMask MachineLayerMask;
    public LayerMask VFXEmptyMachineLayerMask;

    [Header("Machine Placement")]
    public GameObjectField machineHeld;

    [Header("Worker Click")]
    public WorkerField clickedWorker;
    public GameEvent ClickedOnWorker;


    public void RaycastingFromScreenPoint()
    {
        //if(touch)
        if(Input.touchCount == 1)
        {
            //Debug.Log("touchCount == 1 is true");

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(vector2Touch.vector2);

            if (Physics.Raycast(ray, out hit, rayLength, WorkerLayerMask))
            {
                Debug.Log("Worker " + hit.collider.name);
                clickedWorker.worker = hit.collider.gameObject.GetComponent<Worker>();
                ClickedOnWorker.Raise();
            }
            else if (Physics.Raycast(ray, out hit, rayLength, MachineLayerMask))
            {
                Debug.Log("Machine " + hit.collider.name);
            }
            else if (Physics.Raycast(ray, out hit, rayLength, VFXEmptyMachineLayerMask))
            {
                Debug.Log("Empty Machine " + hit.collider.name);
                hit.collider.gameObject.GetComponent<EmptyMachinePlace>().PlaceMachine(machineHeld.gameObjectReference);
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, rayLength, WorkerLayerMask))
                {
                    Debug.Log("Worker " + hit.collider.name);
                    clickedWorker.worker = hit.collider.gameObject.GetComponent<Worker>();
                    ClickedOnWorker.Raise();
                }
                else if (Physics.Raycast(ray, out hit, rayLength, MachineLayerMask))
                {
                    Debug.Log("Machine " + hit.collider.name);
                }
                else if (Physics.Raycast(ray, out hit, rayLength, VFXEmptyMachineLayerMask))
                {
                    Debug.Log("Empty Machine " + hit.collider.name);
                    hit.collider.gameObject.GetComponent<EmptyMachinePlace>().PlaceMachine(machineHeld.gameObjectReference);
                }

            }
        }
    }

    private void OnDisable()
    {
        machineHeld.gameObjectReference = null;
        clickedWorker.worker = null;
    }
}
