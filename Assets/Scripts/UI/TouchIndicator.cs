using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchIndicator : MonoBehaviour
{
    public RectTransform touchIndicator;
    private Animator indicatorAnimator;

    private void Start()
    {
        indicatorAnimator = touchIndicator.GetComponent<Animator>();
    }

    public void OnTouch()
    {
        //indicatorAnimator.SetTrigger("Off");
        if(Input.touchCount == 1)
        {
            touchIndicator.position = Input.touches[0].position;
        }

#if UNITY_EDITOR
        touchIndicator.position = Input.mousePosition;
#endif
        indicatorAnimator.SetTrigger("Touch");
    }
}
