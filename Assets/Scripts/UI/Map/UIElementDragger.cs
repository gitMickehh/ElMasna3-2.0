using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIElementDragger : MonoBehaviour
{
    public bool MouseInput;
    public GameEvent RefreshUIEvent;
    public WorkerUIIconList ListOfWorkerIcons;

    [Header("Map Scroll Space")]
    [Range(0, 100)]
    public float scrollThresholdPercentage;
    float minScrollPixels;
    float maxScrollPixels;

    [Header("Scroll")]
    public GameEvent ScrollUp;
    public GameEvent ScrollDown;
    //public FloatField ScrollDistance;

    private Touch finger;

    public bool dragging = false;
    private bool isSwiping = false;

    private Vector2 swipeDelta;
    private Vector2 startTouch;


    private Vector2 originalPosition;
    private Transform objectToDrag;
    private Image objectToDragImage;

    List<RaycastResult> hitObjects = new List<RaycastResult>();

    private void Start()
    {
        minScrollPixels = Screen.height * (scrollThresholdPercentage / 100f);
        maxScrollPixels = Screen.height - minScrollPixels;
    }

    private void Update()
    {
        if (MouseInput && Input.touchCount == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startTouch = Input.mousePosition;

                objectToDrag = GetDraggableTransformUnderMouse();

                if (objectToDrag != null)
                {
                    dragging = true;
                    objectToDrag.SetAsLastSibling();

                    originalPosition = objectToDrag.localPosition;

                    objectToDragImage = objectToDrag.GetComponent<Image>();
                    objectToDragImage.raycastTarget = false;

                    if (GetComponent<WorkerUIIcon>() != null)
                        ListOfWorkerIcons.DisableRaycastExcept(objectToDrag.GetComponent<WorkerUIIcon>().workerID);

                }
                else
                {
                    dragging = true;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                Reset();

                Transform objectToReplace = null;

                if (objectToDrag != null)
                    objectToReplace = GetDraggableLandingTransformUnderMouse();

                if (objectToReplace != null && objectToDrag != null)
                {
                    objectToDrag.position = objectToReplace.position;
                    objectToReplace.GetComponent<UIMapDraggingContainer>().ReleaseDrag(objectToDrag.gameObject);
                }
                else
                {
                    if (objectToDrag != null)
                    {
                        objectToDrag.localPosition = originalPosition;
                    }
                }

                if (objectToDragImage != null)
                    objectToDragImage.raycastTarget = true;

                objectToDrag = null;
                dragging = false;
                //ScrollDistance.SetValue(0);
                ListOfWorkerIcons.EnableRaycastTargets();
            }

        }

        if (Input.touchCount == 1)
        {
            startTouch = Input.touches[0].position;

            finger = Input.touches[0];

            if (finger.phase == TouchPhase.Began)
            {
                objectToDrag = GetDraggableTransformUnderMouse();

                if (objectToDrag != null)
                {
                    dragging = true;
                    objectToDrag.SetAsLastSibling();

                    originalPosition = objectToDrag.localPosition;

                    objectToDragImage = objectToDrag.GetComponent<Image>();
                    objectToDragImage.raycastTarget = false;

                    if (GetComponent<WorkerUIIcon>() != null)
                        ListOfWorkerIcons.DisableRaycastExcept(objectToDrag.GetComponent<WorkerUIIcon>().workerID);

                }
            }
            else if (finger.phase == TouchPhase.Ended || finger.phase == TouchPhase.Canceled)
            {
                Reset();

                Transform objectToReplace = GetDraggableLandingTransformUnderMouse();

                if (objectToReplace != null && objectToDrag != null)
                {
                    objectToDrag.position = objectToReplace.position;
                    objectToReplace.GetComponent<UIMapDraggingContainer>().ReleaseDrag(objectToDrag.gameObject);
                }
                else
                {
                    if (objectToDrag != null)
                    {
                        objectToDrag.localPosition = originalPosition;
                    }

                }

                if (objectToDragImage != null)
                    objectToDragImage.raycastTarget = true;

                objectToDrag = null;
                dragging = false;

                //ScrollDistance.SetValue(0);
                ListOfWorkerIcons.EnableRaycastTargets();
            }

        }

        if (dragging)
        {
            swipeDelta = Vector2.zero;

            if (objectToDrag != null)
            {
                if (MouseInput && Input.touchCount == 0)
                    objectToDrag.position = Input.mousePosition;

                if (Input.touchCount == 1)
                {
                    objectToDrag.position = finger.position;
                }

                if (objectToDrag.position.y <= minScrollPixels)
                {
                    ScrollDown.Raise();
                }
                else if (objectToDrag.position.y >= maxScrollPixels)
                {
                    ScrollUp.Raise();
                }
            }

            else
            {
                if (Input.touchCount == 1)
                    swipeDelta = Input.touches[0].position - startTouch;
                else if (Input.GetMouseButton(0))
                    swipeDelta = (Vector2)Input.mousePosition - startTouch;

                if (swipeDelta.y > 0)
                {
                    ScrollDown.Raise();
                }
                else if (swipeDelta.y < 0)
                {
                    ScrollUp.Raise();
                }

                Reset();
            }
        }
    }

    private GameObject GetObjectUnderMouse()
    {
        var pointer = new PointerEventData(EventSystem.current);

        if (MouseInput && Input.touchCount == 0)
            pointer.position = Input.mousePosition;

        if (Input.touchCount == 1)
        {
            pointer.position = Input.touches[0].position;
        }

        EventSystem.current.RaycastAll(pointer, hitObjects);

        if (hitObjects.Count == 0) return null;

        return hitObjects[0].gameObject;
    }

    private Transform GetDraggableTransformUnderMouse()
    {
        GameObject clickedObject = GetObjectUnderMouse();
        WorkerUIIcon workerIconComponent = null;

        if (clickedObject)
            workerIconComponent = clickedObject.GetComponent<WorkerUIIcon>();
    
        if (!clickedObject || !workerIconComponent)
        {
            return null;
        }

        if (clickedObject != null && workerIconComponent.UIDraggableType.draggable)
        {
            //.parent.parent to control the whole UI room

            clickedObject.transform.parent.parent.SetAsLastSibling();
            workerIconComponent.AnimatorTrigger();
            AudioManager.instance.Play("GrabUI");

            return clickedObject.transform;
        }

        return null;
    }

    private Transform GetDraggableLandingTransformUnderMouse()
    {
        GameObject clickedObject = GetObjectUnderMouse();

        if (clickedObject.GetComponent<UIMapDraggingContainer>() == null)
            return null;

        if (clickedObject != null)
        {
            return clickedObject.transform;
        }

        return null;
    }

    private void Reset()
    {
        swipeDelta = Vector2.zero;
    }
}

