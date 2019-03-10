using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
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
    [Range(0, 180)]
    public int horizontalSwipeDeadzone = 65;
    [Tooltip("In Pixels")]
    [Range(0, 180)]
    public int VerticalSwipeDeadzone = 25;
    public float tapForgiveness = 10; //the area a finger can move and still counts as tap
    public FloatField swipeMagnitude;
    public FloatField PinchMagnitude;
    public Vector2Field tapPosition;

    private void Update()
    {
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
        else if (tapCounter >= 2)
        {
            doubleTap.Raise();
            tapCounter = 0;
            currentTimer = 0;
        }
        #endregion

        #region Mobile Input
#if UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                //tap = true;
                //tap.Raise();
                isDraging = true;
                tapCounter++;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended)
            {
                //release tap
                var t = Input.touches[0].position;
                tapPosition.SetVector2(t);

                Debug.Log("touches ended\n" + "position is: " + t);

                Debug.Log("Start touch: " + startTouch);

                if (Mathf.Abs(t.x - startTouch.x) <= tapForgiveness && Mathf.Abs(t.y - startTouch.y) <= tapForgiveness)
                {
                    Debug.Log("Tap touch Mobile Input");
                    tap.Raise();
                }

                Reset();
            }
        }
        else if (Input.touchCount > 1)
            isDraging = false;
#endif
        #endregion

        #region Standalone Input
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            //tap = true;
            //tap.Raise();
            isDraging = true;
            tapCounter++;
            startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //release tap
            if (tapCounter == 1)
            {
                Debug.Log("mouse button ended\n" + "position is: " + Input.mousePosition);

                Debug.Log("Start touch: " + startTouch);


                if (Mathf.Abs(Input.mousePosition.x - startTouch.x) <= tapForgiveness &&
                    Mathf.Abs(Input.mousePosition.y - startTouch.y) <= tapForgiveness)
                {
                    Debug.Log("Tap touch mouse Input");
                    tap.Raise();
                }
            }

            Reset();
        }
#endif
        #endregion

        //calculate the distance
        swipeDelta = Vector2.zero;
        if (isDraging)
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

            if (Mathf.Abs(x) > Mathf.Abs(y))
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

    }

    private void Reset()
    {

        startTouch = swipeDelta = Vector2.zero;
        isDraging = false;
    }
}
