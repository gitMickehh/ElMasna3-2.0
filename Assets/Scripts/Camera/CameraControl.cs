using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
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

    [Header("Camera Range")]
    public float minimumHeight;
    public float maximumHeight;
    private float clampY;

    [Header("Zoom Orthographic")]
    public float zoomOutMin;
    public float zoomOutMax;

    [Header("Zoom Prespective")]
    public float minimumZoom;
    public float maximumZoom;
    private float clampZ;

    private IEnumerator Lerping()
    {
        while(shouldLerp)
        {
            transform.position = Lerp(startPosition, endPosition, timeStartedLerping, lerpTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private void FixedUpdate()
    {
        clampY = Mathf.Clamp(transform.position.y,minimumHeight,maximumHeight);
        transform.position = new Vector3(transform.position.x,clampY,transform.position.z);

        clampZ = Mathf.Clamp(transform.position.z, minimumZoom, maximumZoom);
        transform.position = new Vector3(transform.position.x, transform.position.y, clampZ);
    }

    public void TraverseUp()
    {
        timeStartedLerping = Time.time;
        startPosition = transform.position;
        endPosition = transform.position + (Vector3.up * swipeMultiplier * swipeMagnitude.GetValue());
        shouldLerp = true;

        StopAllCoroutines();
        StartCoroutine(Lerping());
    }

    public void TraverseDown()
    {
        timeStartedLerping = Time.time;
        startPosition = transform.position;
        endPosition = transform.position + (Vector3.up * -swipeMultiplier * swipeMagnitude.GetValue());
        shouldLerp = true;

        StopAllCoroutines();
        StartCoroutine(Lerping());
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
                zoomOutMin, zoomOutMax);
        }
        else
        {
            //transform.position = new Vector3(transform.position.x, transform.position.y,
            //    Mathf.Clamp(transform.position.z + (pinchMultiplier * pinchMagnitude.GetValue()),
            //    minimumZoom, maximumZoom));

            transform.position = new Vector3(transform.position.x, transform.position.y,
                transform.position.z + (pinchMultiplier * pinchMagnitude.GetValue()));
        }
    }

    Vector3 Lerp(Vector3 start, Vector3 end, float timeStartedLerping, float lerpTime = 1)
    {
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        var result = Vector3.Lerp(start,end,percentageComplete);

        if (percentageComplete > 0.9f)
            shouldLerp = false;

        return result;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(transform.position.x,minimumHeight,transform.position.z),
            new Vector3(transform.position.x,maximumHeight,transform.position.z));

        Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y, minimumZoom),
            new Vector3(transform.position.x, transform.position.y, maximumZoom));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, minimumHeight, transform.position.z),
            0.5f);
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, maximumHeight, transform.position.z),
            0.5f);

        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, minimumZoom),
            0.5f);
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y, maximumZoom),
            0.5f);
    }
}
