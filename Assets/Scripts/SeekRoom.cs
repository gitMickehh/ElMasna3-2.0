using Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekRoom : MonoBehaviour
{
    public WayPoint wayPointCurrent;
    public float speed = 1f;

    Rigidbody myBody;

    [SerializeField]
    [GreyOut]
    Transform targetPos;

    [SerializeField]
    [GreyOut]
    bool walking = false;

    [SerializeField]
    [GreyOut]
    bool arrived = false;

    [SerializeField]
    [GreyOut]
    bool rotateOnce = true;

    WayPoint targetWayPoint;

    void Awake()
    {
        myBody = GetComponent<Rigidbody>();
    }

    public void SwitchRoom(WayPoint wayPointTarget)
    {
        Transform door1 = wayPointCurrent.doorPosition;

        targetWayPoint = wayPointTarget;

        Walk(transform, door1);

    }

    public void Walk(Transform startPos, Transform endPos)
    {
        targetPos = endPos;
        walking = true;
    }

    void FixedUpdate()
    {
        if (walking == true)
        {

            float distance = Vector3.Distance(targetPos.position, transform.position);

            if (rotateOnce)
            {
                transform.forward = -wayPointCurrent.doorPosition.forward;
                rotateOnce = false;
            }

            if (distance > 3)
            {
                float step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, targetPos.position, step);

            }

            else if (!arrived)
            {
                walking = false;

                transform.position = new Vector3(targetWayPoint.doorPosition.position.x, targetWayPoint.doorPosition.position.y
                    , targetWayPoint.doorPosition.position.z);

                transform.forward = targetWayPoint.doorPosition.forward;

                Walk(targetWayPoint.doorPosition, targetWayPoint.WayPointTransform);

                arrived = true;
            }
            else if (arrived)
            {
                walking = false;
            }

        }
        else if (!walking && arrived)
        {
            wayPointCurrent = targetWayPoint;
            arrived = false;
            rotateOnce = true;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.forward*10);
    }
}
