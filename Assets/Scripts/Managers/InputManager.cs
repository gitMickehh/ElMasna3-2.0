using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    Touch[] fingers;
    private Vector2 startTouch;
    private Vector2 swipeDelta;
    private bool isDraging;
    private int tapCounter = 0;
    private float currentTimer = 0;
    public float doubleTapTime = 0.6f;

    [Header("Input Events")] 
    public GameEvent tap;
    public GameEvent doubleTap;
    public GameEvent swipeLeft;
    public GameEvent swipeRight;
    public GameEvent swipeUp;
    public GameEvent swipeDown;
    
    [Header("Input Senstivity")]
    [Tooltip("In Pixels")]
    [Range(0,180)]
    public int horizontalSwipeDeadzone = 65;
    [Tooltip("In Pixels")]
    [Range(0, 180)]
    public int VerticalSwipeDeadzone = 25;
    public FloatField swipeMagnitude;
    public FloatField PinchMagnitude;

    private void Update()
    {
        //tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;
        #region doubleTapCheck
        if (currentTimer >= doubleTapTime)
        {
            tapCounter = 0;
            currentTimer = 0;
        }
        else if (tapCounter == 0)
        {
            currentTimer = 0;
        }
        else if (tapCounter == 1)
        {
            currentTimer += Time.deltaTime;
        }
        else if(tapCounter >= 2)
        {
            doubleTap.Raise();
            tapCounter = 0;
            currentTimer = 0;
        }
        #endregion

        #region Standalone Input
        if (Input.GetMouseButtonDown(0))
        {
            //tap = true;
            tap.Raise();
            isDraging = true;
            tapCounter++;
            startTouch = Input.mousePosition;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            Reset();
        }
        #endregion
        #region Mobile Input
        if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                //tap = true;
                tap.Raise();
                isDraging = true;
                tapCounter++;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                Reset();
            }
        }
        else if(Input.touchCount > 1)
            isDraging = false;
        #endregion

        //calculate the distance
        swipeDelta = Vector2.zero;
        if(isDraging)
        {
            if (Input.touchCount == 1)
                swipeDelta = Input.touches[0].position - startTouch;
            else if (Input.GetMouseButton(0))
                swipeDelta = (Vector2)Input.mousePosition - startTouch;

        }

        //deadzone
        if (Mathf.Abs(swipeDelta.x) >= horizontalSwipeDeadzone || 
            Mathf.Abs(swipeDelta.y) >= VerticalSwipeDeadzone)
        {
            swipeMagnitude.SetValue(swipeDelta.magnitude);
            //direciton
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if(Mathf.Abs(x) > Mathf.Abs(y))
            {
                //Left or Right
                if (x < 0)
                {
                    //swipeLeft = true;
                    swipeLeft.Raise();
                }
                else
                    swipeRight.Raise();
            }
            else
            {
                //Up or Down
                if (y < 0)
                    swipeDown.Raise();
                else
                    swipeUp.Raise();
            }

            Reset();
        }

        //pinch
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;
            PinchMagnitude.SetValue(difference);

            Reset();
        }

        //zoom mouse
        //PinchMagnitude.SetValue(Input.GetAxis("Mouse ScrollWheel") * 20);
        
    }

    private void Reset()
    {

        startTouch = swipeDelta = Vector2.zero;
        isDraging = false;
    }
}
