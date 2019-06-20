using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSawying2 : MonoBehaviour
{
    //lerping
    private bool shouldLerp = false;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float timeStartedLerping;
    public float lerpTime = 1;

    Vector3 originalPosition;
    Vector3 targetPos;

    public float shakeRange = 2;

    private void Start()
    {
        originalPosition = transform.position;
        targetPos = GetRandomPosition(shakeRange);
        endPosition = targetPos;

        shouldLerp = true;
        timeStartedLerping = Time.time;
        startPosition = transform.position;

        StartCoroutine(Lerping());
    }

    private Vector3 GetRandomPosition(float range)
    {
        float x = Random.Range(originalPosition.x - range, originalPosition.x+range);
        float y = Random.Range(originalPosition.y - range,originalPosition.y+range);
        float z = Random.Range(originalPosition.z - range,originalPosition.z+range);

        return new Vector3(x,y,z);
    }

    private IEnumerator Lerping()
    {
        while(true)
        {
            while (shouldLerp)
            {
                transform.position = Lerp(startPosition, endPosition, timeStartedLerping, lerpTime);
                yield return new WaitForEndOfFrame();
            }

            timeStartedLerping = Time.time;
            startPosition = transform.position;
            targetPos = GetRandomPosition(shakeRange);
            endPosition = targetPos;
            shouldLerp = true;
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
        }

        return result;
    }

    private void OnDrawGizmos()
    {
        
        if(originalPosition == null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, shakeRange);
        }
        else
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(originalPosition, shakeRange);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(targetPos != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(targetPos,0.3f);
            Gizmos.DrawRay(transform.position,targetPos);
        }
    }
}
