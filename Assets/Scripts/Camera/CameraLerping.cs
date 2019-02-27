using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerping : MonoBehaviour
{
    public BoolField movingCamera;

    //lerping
    private bool shouldLerp = false;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float timeStartedLerping;
    public float lerpTime = 1;

    //quality of life
    float lerpTimeTemp;

    //old position
    Vector3 originalPosition;
    [SerializeField]
    [Attributes.GreyOut]
    private Transform orientationPosition;
    bool atOrientation;

    CameraControl camControlScript;

    private void Start()
    {
        originalPosition = transform.position;
        lerpTimeTemp = lerpTime;
        camControlScript = GetComponent<CameraControl>();

        StartCoroutine(FindOrientationPosition());
    }

    IEnumerator FindOrientationPosition()
    {
        yield return new WaitForEndOfFrame();
        orientationPosition = FindObjectOfType<OrientationBuilding>().RoomCamera;
    }

    public void SwitchView()
    {
        StopAllCoroutines();
        shouldLerp = true;
        movingCamera.BoolValue = true;

        timeStartedLerping = Time.time;
        startPosition = transform.position;

        if (atOrientation)
        {
            endPosition = originalPosition;
            atOrientation = false;
            lerpTimeTemp = lerpTime;
            lerpTime /= 2;
        }
        else
        {
            endPosition = orientationPosition.position;
            originalPosition.y = transform.position.y;
            lerpTime = lerpTimeTemp;

            atOrientation = true;
        }

        StartCoroutine(Lerping());
    }

    private IEnumerator Lerping()
    {
        while (shouldLerp)
        {
            transform.position = Lerp(startPosition, endPosition, timeStartedLerping, lerpTime);
            yield return new WaitForEndOfFrame();
        }
    }

    Vector3 Lerp(Vector3 start, Vector3 end, float timeStartedLerping, float lerpTime = 1)
    {
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        var result = Vector3.Lerp(start, end, percentageComplete);

        if (percentageComplete > 0.9f)
        {
            shouldLerp = false;
            movingCamera.BoolValue = false;
        }

        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if(orientationPosition != null)
        {
            Gizmos.DrawLine(transform.position, orientationPosition.position);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(orientationPosition.position, 0.5f);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
