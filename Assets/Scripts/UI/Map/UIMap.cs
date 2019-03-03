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

    [Header("Scroll")]
    public float scrollSpeed;
    public FloatField scrollDistance;

    private void OnEnable()
    {
        UIOn.Raise();
        FillInMap();
    }

    private void OnDisable()
    {
        UIOff.Raise();
    }

    public void FillInMap()
    {
        if(FloorUIParent.childCount >= listOfFloors.Items.Count)
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
            //FloorUIParent.GetComponent<VerticalLayoutGroup>().spacing = Height;
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
        var x = FloorUIParent.localPosition;
        x.y -= scrollSpeed;
        FloorUIParent.localPosition = x;

        scrollDistance.AddValue(-scrollSpeed);
    }

    public void OnScrollDown()
    {
        var x = FloorUIParent.localPosition;
        x.y += scrollSpeed;
        FloorUIParent.localPosition = x;

        scrollDistance.AddValue(scrollSpeed);
    }
}
