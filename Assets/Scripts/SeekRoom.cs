using Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekRoom : MonoBehaviour
{
    public WayPoint wayPointCurrent;
    public float speed = 0.05f;

    Rigidbody myBody;
    Vector3 direction;

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

        direction = endPos.position - startPos.position;
        direction.Normalize();
        direction = new Vector3(direction.x, 0, direction.z);

        walking = true;
    }

    void FixedUpdate()
    {
        if (walking == true)
        {
            float distance = Vector3.Distance(targetPos.position, transform.position);

            if (distance > 3)
            {
                if (rotateOnce)
                {
                    transform.forward = -wayPointCurrent.doorPosition.forward;
                    rotateOnce = false;
                }

                myBody.MovePosition(transform.position + (direction * speed));
            }

            else if (!arrived)
            {
                walking = false;

                transform.position = new Vector3(targetWayPoint.doorPosition.position.x, targetWayPoint.doorPosition.position.y
                    , targetWayPoint.doorPosition.position.z);

                transform.forward = targetWayPoint.doorPosition.forward;

                Walk(targetWayPoint.doorPosition, targetWayPoint.WayPointTransform);

                arrived = true;
                //walking = false;
            }
            else if (arrived)
            {
                //wayPointCurrent = targetWayPoint;
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
