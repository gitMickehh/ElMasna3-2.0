using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMap : MonoBehaviour
{
    [Header("Events")]
    public GameEvent UIOn;
    public GameEvent UIOff;

    [Header("UI Instantiation")]
    public RectTransform FloorUIParent;
    public GameObject FloorUIPanel;

    [Header("Floors")]
    public FloorList listOfFloors;
    public WorkerUIIconList iconsList;

    [Header("Scroll")]
    public float scrollSpeed;
    public FloatField scrollDistance;

    private float localYStart;
    private float localYEnd;

    private void Start()
    {
        iconsList.Items.Clear();
        FillInMap();
        localYStart = FloorUIParent.localPosition.y;
        localYEnd = FloorUIParent.localPosition.y + (FloorUIParent.rect.height * (FloorUIParent.childCount - 1));

    }

    private void OnEnable()
    {
        UIOn.Raise();
    }

    private void OnDisable()
    {
        UIOff.Raise();
    }

    public void FillInMap()
    {
        if (FloorUIParent.childCount >= listOfFloors.Items.Count)
        {
            var children = FloorUIParent.GetComponentsInChildren<UIFloor>();
            for (int i = 0; i < children.Length; i++)
            {
                children[i].UpdateFloor();
            }
        }
        else
        {
            float Height = FloorUIParent.rect.height;
            int iMin = FloorUIParent.childCount;

            for (int i = listOfFloors.Items.Count - 1; i >= iMin; i--)
            {
                var f = Instantiate(FloorUIPanel, FloorUIParent);

                f.GetComponent<RectTransform>().localPosition = Vector3.up * Height * i;

                var fCompo = f.GetComponent<UIFloor>();
                fCompo.realFloor = listOfFloors.Items[i];
                fCompo.UpdateFloor();
            }
        }


    }

    public void OnScrollUp()
    {
        var positionLocal = FloorUIParent.localPosition;
        positionLocal.y -= scrollSpeed;
        positionLocal.y = Mathf.Clamp(positionLocal.y, -localYEnd, localYStart);

        //Debug.Log("scroll up: " + positionLocal.y);
        FloorUIParent.localPosition = positionLocal;

        scrollDistance.AddValue(-scrollSpeed);
    }

    public void OnScrollDown()
    {
        var positionLocal = FloorUIParent.localPosition;
        positionLocal.y += scrollSpeed;
        positionLocal.y = Mathf.Clamp(positionLocal.y, -localYEnd, localYStart);

        //Debug.Log("scroll down: " + positionLocal.y);
        FloorUIParent.localPosition = positionLocal;

        scrollDistance.AddValue(scrollSpeed);
    }
}
