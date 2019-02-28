using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public BoolField movingCamera;

    [Header("Input Senstivity")]
    public FloatField swipeMagnitude;
    public float swipeMultiplier = 0.15f;
    public FloatField pinchMagnitude;
    public float pinchMultiplier = 0.05f;

    //lerping
    private bool shouldLerp = false;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float timeStartedLerping;
    public float lerpTime = 1;

    //raycasting
    RaycastingForFloor rcaster;

    [Header("Camera Properties")]
    public CameraProperties cameraPorperties;

    //height
    private float clampY;
    private bool traverseVertical = true;

    //zoom
    private float clampZ;

    private IEnumerator Lerping()
    {
        while(shouldLerp)
        {
            transform.position = Lerp(startPosition, endPosition, timeStartedLerping, lerpTime);

            clampY = Mathf.Clamp(transform.position.y, cameraPorperties.minimumHeight, cameraPorperties.maximumHeight);
            transform.position = new Vector3(transform.position.x, clampY, transform.position.z);

            yield return new WaitForEndOfFrame();
        }
    }

    public void TraverseUp()
    {
        if(traverseVertical)
        {
            timeStartedLerping = Time.time;
            startPosition = transform.position;
            endPosition = transform.position + (Vector3.up * swipeMultiplier * swipeMagnitude.GetValue());
            shouldLerp = true;

            StopAllCoroutines();
            StartCoroutine(Lerping());
        }
    }

    public void TraverseDown()
    {
        if(traverseVertical)
        {
            timeStartedLerping = Time.time;
            startPosition = transform.position;
            endPosition = transform.position + (Vector3.up * -swipeMultiplier * swipeMagnitude.GetValue());
            shouldLerp = true;
            movingCamera.BoolValue = true;

            StopAllCoroutines();
            StartCoroutine(Lerping());
        }
    }

    public void TraverseVertical(bool cond)
    {
        traverseVertical = cond;
    }

    public void StopTraversing()
    {
        shouldLerp = false;
    }

    public void Zoom()
    {
        if(Camera.main.orthographic)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - 
                (pinchMultiplier * pinchMagnitude.GetValue()),
                cameraPorperties.zoomOutMin, cameraPorperties.zoomOutMax);
        }
        else
        {
            Vector3 DesiredPosition = new Vector3(transform.position.x, transform.position.y,
                transform.position.z + (pinchMultiplier * pinchMagnitude.GetValue()));

            clampZ = Mathf.Clamp(DesiredPosition.z, cameraPorperties.minimumZoom, cameraPorperties.maximumZoom);
            transform.position = new Vector3(transform.position.x, transform.position.y, clampZ);
            Debug.Log(clampZ);
        }
    }

    Vector3 Lerp(Vector3 start, Vector3 end, float timeStartedLerping, float lerpTime = 1)
    {
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        var result = Vector3.Lerp(start,end,percentageComplete);

        if (percentageComplete > 0.9f)
        {
            movingCamera.BoolValue = true;
            shouldLerp = false;
        }

        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(transform.position.x, cameraPorperties.minimumHeight, transform.position.z),
            new Vector3(transform.position.x, cameraPorperties.maximumHeight,transform.position.z));

        Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y, cameraPorperties.minimumZoom),
            new Vector3(transform.position.x, transform.position.y, cameraPorperties.maximumZoom));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, cameraPorperties.minimumHeight, transform.position.z),
            0.5f);
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, cameraPorperties.maximumHeight, transform.position.z),
            0.5f);

        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, cameraPorperties.minimumZoom),
            0.5f);
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, cameraPorperties.maximumZoom),
            0.5f);
    }
}
