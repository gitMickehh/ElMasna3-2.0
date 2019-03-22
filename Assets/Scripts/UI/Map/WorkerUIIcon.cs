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

    public void AnimatorTrigger()
    {
        GetComponent<Animator>().SetTrigger("Interact");
    }

    /// <summary>
    /// if there is a container already, it swaps with the other workerUIIcon.
    /// it also places the icon by its transform.
    /// </summary>
    public void GiveContainerRef(UIMapDraggingContainer newContainer)
    {
        if(newContainer.workerImage == null)
        {
            newContainer.workerImage = gameObject;
            container.workerImage = null;
        }
        else
        {
            //new container is not empty


            Debug.Log("Switching Containers");
            GameObject temp = newContainer.workerImage;
            var otherWorkerIcon = temp.GetComponent<WorkerUIIcon>();

            container.workerImage = temp;
            otherWorkerIcon.container = container;

            temp.transform.SetParent(container.transform);
            temp.transform.localPosition = new Vector2();
            otherWorkerIcon.AnimatorTrigger();

            newContainer.workerImage = gameObject;
        }

        container = newContainer;
        transform.SetParent(container.transform);
        transform.localPosition = new Vector2();
        AnimatorTrigger();
    }
}

