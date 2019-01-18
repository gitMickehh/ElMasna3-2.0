using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchZoom : MonoBehaviour
{
    public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
    public float minPerspectiveFOV = 33f;
    public float maxPerspectiveFOV = 66f;

    public float orthoZoomSpeed = 0.05f;        // The rate of change of the orthographic size in orthographic mode.
    Camera cam;
    public Camera []cameraChild;
    public float minOrthographicSize = 8.7f;
    public float maxOrthographicSize = 15f;

    Vector3 cameraPrespectivePosition;

    private void Start()
    {
        cam = transform.GetComponent<Camera>();
        cameraChild = transform.GetComponentsInChildren<Camera>();

    }

    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
            
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
            
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            
            if (cam.orthographic)
            {
                cam.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                cam.orthographicSize = Mathf.Max(cam.orthographicSize, minOrthographicSize);
                cam.orthographicSize = Mathf.Min(cam.orthographicSize, maxOrthographicSize);

                cameraChild[1].orthographicSize = cam.orthographicSize;
               
            }

            else
            {
                /*
                // Otherwise change the field of view based on the change in distance between the touches.
                camera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

                // Clamp the field of view to make sure it's between 0 and 180.
                camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, 0.1f, 179.9f);

                camera.fieldOfView = Mathf.Max(camera.fieldOfView, minPerspectiveFOV);
                camera.fieldOfView = Mathf.Min(camera.fieldOfView, maxPerspectiveFOV);

                cameraChild[1].fieldOfView = camera.fieldOfView;
                */

                cameraPrespectivePosition = cam.transform.position;
                cameraPrespectivePosition.z += (deltaMagnitudeDiff * perspectiveZoomSpeed);

                cameraPrespectivePosition.z = Mathf.Max(cameraPrespectivePosition.z, minPerspectiveFOV);
                cameraPrespectivePosition.z = Mathf.Min(cameraPrespectivePosition.z, maxPerspectiveFOV);

                cam.transform.position = cameraPrespectivePosition;
            }

        }
    }
}
