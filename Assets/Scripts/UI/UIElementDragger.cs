using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIElementDragger : MonoBehaviour
{
    //public const string DRAGGABLE_TAG = "UIDragable";
    //public const string LANDING_DRAGGABLE_TAG = "UILandingDraggable";
    public bool MouseInput;
    public GameEvent RefreshUIEvent;

    [Header("Map Scroll Space")]
    [Range(0, 100)]
    public float scrollThresholdPercentage;
    float minScrollPixels;
    float maxScrollPixels;

    [Header("Scroll")]
    public GameEvent ScrollUp;
    public GameEvent ScrollDown;
    public FloatField ScrollDistance;

    private Touch finger;

    private bool dragging = false;

    private Vector2 originalPosition;
    private Transform objectToDrag;
    private Image objectToDragImage;

    //private Transform currentObjectReplaced;

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
                objectToDrag = GetDraggableTransformUnderMouse();

                if (objectToDrag != null)
                {
                    dragging = true;
                    objectToDrag.SetAsLastSibling();

                    originalPosition = objectToDrag.position;
                    objectToDragImage = objectToDrag.GetComponent<Image>();
                    objectToDragImage.raycastTarget = false;

                    //if (currentObjectReplaced != null)
                    //    currentObjectReplaced.GetComponent<DraggableUI>().OnThisOne = false;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                Transform objectToReplace = GetDraggableLandingTransformUnderMouse();

                if (objectToReplace != null && objectToDrag != null)
                {
                    objectToDrag.position = objectToReplace.position;
                    //objectToDrag.SetParent(objectToReplace);
                    //objectToReplace.GetComponent<Image>().enabled = false;
                    //objectToReplace.GetComponent<DraggableUI>().OnThisOne = true;
                    //currentObjectReplaced = objectToReplace;

                    objectToReplace.GetComponent<UIMachine>().ChangeWorker(objectToDrag.gameObject);
                }
                else
                {
                    if (objectToDrag != null)
                    {
                        Vector2 newPosition = originalPosition;
                        newPosition.y += ScrollDistance.GetValue();

                        objectToDrag.position = newPosition;
                    }
                }

                if (objectToDragImage != null)
                    objectToDragImage.raycastTarget = true;

                objectToDrag = null;
                dragging = false;
                //RefreshUIEvent.Raise();
                ScrollDistance.SetValue(0);
            }

        }

        if (Input.touchCount == 1)
        {
            finger = Input.touches[0];

            if (finger.phase == TouchPhase.Began)
            {
                objectToDrag = GetDraggableTransformUnderMouse();

                if (objectToDrag != null)
                {
                    dragging = true;
                    objectToDrag.SetAsLastSibling();

                    originalPosition = objectToDrag.position;
                    objectToDragImage = objectToDrag.GetComponent<Image>();
                    objectToDragImage.raycastTarget = false;
                }
            }
            else if (finger.phase == TouchPhase.Ended || finger.phase == TouchPhase.Canceled)
            {
                Transform objectToReplace = GetDraggableLandingTransformUnderMouse();

                if (objectToReplace != null && objectToDrag != null)
                {
                    objectToDrag.position = objectToReplace.position;
                    //objectToDrag.SetParent(objectToReplace);
                    //objectToReplace.GetComponent<Image>().enabled = false;
                    //objectToReplace.GetComponent<DraggableUI>().OnThisOne = true;
                    //currentObjectReplaced = objectToReplace;
                    objectToReplace.GetComponent<UIMachine>().ChangeWorker(objectToDrag.gameObject);
                }
                else
                {
                    if (objectToDrag != null)
                    {
                        Vector2 newPosition = originalPosition;
                        newPosition.y += ScrollDistance.GetValue();

                        objectToDrag.position = newPosition;
                    }
                        
                }

                if (objectToDragImage != null)
                    objectToDragImage.raycastTarget = true;

                objectToDrag = null;
                dragging = false;
                //RefreshUIEvent.Raise();

                ScrollDistance.SetValue(0);
            }

        }

        if (dragging)
        {
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

        if (clickedObject.GetComponent<WorkerUIIcon>() == null)
            return null;

        //if(clickedObject != null && clickedObject.tag == DRAGGABLE_TAG)
        if (clickedObject != null && clickedObject.GetComponent<WorkerUIIcon>().UIDraggableType.draggable)
        {
            return clickedObject.transform;
        }

        return null;
    }

    private Transform GetDraggableLandingTransformUnderMouse()
    {
        GameObject clickedObject = GetObjectUnderMouse();

        if (clickedObject.GetComponent<UIMachine>() == null)
            return null;

        //if (clickedObject != null && clickedObject.tag == LANDING_DRAGGABLE_TAG)
        if (clickedObject != null)
        {
            return clickedObject.transform;
        }

        return null;
    }

}

