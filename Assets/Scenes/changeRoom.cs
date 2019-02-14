using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeRoom : MonoBehaviour
{
    public WayPoint wayPointCurrent;
    public float speed = 5f;

    Rigidbody rigidbody;
    Vector3 direction;

    [SerializeField]
    Transform targetPos;

    [SerializeField]
    bool walking = false;
    WayPoint targetPoint;
    //WorkerState workerState;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void ChangeRoom(WayPoint wayPointTarget)
    {
        Transform door1 = wayPointCurrent.doorPosition;
        //Transform door2 = wayPointTarget.doorPosition;
        targetPoint = wayPointTarget;

        Walk(transform, door1);

        //if (!walking)
        //{
        //    //transform.position = new Vector3(door2.position.x, door2.position.y, door2.position.z);
        //    //Walk(door2, wayPointTarget.WayPointTransform);
        //}
    }


    public void Walk(Transform startPos, Transform endPos)
    {
        targetPos = endPos;
        Debug.Log("endPos: " + endPos);

        direction = endPos.position - startPos.position;
        direction.Normalize();
        direction = new Vector3(direction.x, 0, direction.z);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.15F); //making worker facing the movement

        walking = true;
    }


    void FixedUpdate()
    {
        if (walking == true)
        {
            float distance = Vector3.Distance(targetPos.position, transform.position);
            //Debug.Log("distance: " + distance);
            if (distance > 3)
            {
                Debug.Log("direction: " + direction);
                rigidbody.MovePosition(transform.position + (direction * speed));
            }
            else
            {
                walking = false;
                Debug.Log("walking = false");

                transform.position = new Vector3(targetPoint.doorPosition.position.x, targetPoint.doorPosition.position.y
                    , targetPoint.doorPosition.position.z);
                Walk(targetPoint.doorPosition, targetPoint.WayPointTransform);

            }
            //workerState = WorkerState.Idle;

        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.forward*10);
    }
}
