using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WorkerUIIcon : MonoBehaviour
{
    public DraggableType UIDraggableType;
    public int workerID;
    public UIMapDraggingContainer container;

    [SerializeField]
    private WorkerUIIconList listOfWIcons;

    private void Start()
    {
        listOfWIcons.Add(this);
    }

    private void OnApplicationQuit()
    {
        listOfWIcons.Remove(this);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
            listOfWIcons.Remove(this);
        else
            listOfWIcons.Add(this);
    }

    public void Copy(WorkerUIIcon other)
    {
        UIDraggableType = other.UIDraggableType;
        workerID = other.workerID;
        container = other.container;
    }

    /// <summary>
    /// if there is a container already, it swaps with the other workerUIIcon.
    /// it also places the icon by its transform.
    /// </summary>
    public void GiveMachineRef(UIMapDraggingContainer newContainer)
    {

    }
}

