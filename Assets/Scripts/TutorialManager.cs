using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    //Welcome player.
    //Introduce Swiping Left/Right (Horizontal) Rotating Floor
    [Header("Lists")]
    public WorkerList workersList;

    [Header("Language")]
    public LanguageProfile lang;

    [Header("Pointer")]
    public Transform parentToPointers;
    public GameObject animatedPointer;
    public GameObject nonAnimatedPointer;
    private Animator pointerAniamtor;
    public List<string> pointerAnimatorSteps;

    [Header("Tutorial Steps")]
    [Attributes.GreyOut]
    public int tutorialStage = 0;
    public float waitTimeBetweenTuts = 1.5f;
    [SerializeField]
    private bool[] tutorialStepsDone;

    [Tooltip("To add these events on the corresponding buttons")]
    [Header("Events")]
    public GameEvent OrientationButtonPressEvent;
    public GameEvent HireButtonPressEvent;

    //needed objects
    [Header("Buttons")]
    [SerializeField]
    [Attributes.GreyOut]
    private Button OrientationButton;
    [SerializeField]
    [Attributes.GreyOut]
    private Button StoreButton;
    [SerializeField]
    [Attributes.GreyOut]
    private Button HireButton;

    [Header("OrientationWorkers")]
    [SerializeField]
    float averageWorkerWidth = -15f; //x
    [SerializeField]
    float averageWorkerHeight = 50f; //y
    [SerializeField]
    [Attributes.GreyOut]
    List<Worker> workersInOrientation;

    private void Start()
    {
        pointerAniamtor = animatedPointer.GetComponent<Animator>();
        OrientationButton = FindObjectOfType<OrientationButtonIcon>().GetComponent<Button>();
        StoreButton = GameObject.Find("StoreButton").GetComponent<Button>();
        tutorialStepsDone = new bool[10];

        StartCoroutine(WaitTime(tutorialStage));
    }

    private IEnumerator WaitTime(int i)
    {
        yield return new WaitForSeconds(waitTimeBetweenTuts);

        switch (i)
        {
            case 0:
                SwipeHorizontal();
                break;
            case 1:
                OrientationButtonTut();
                break;
            case 2:
                OrientationWorkerTouch();
                break;
            case 3:
                //s
                break;
            default:
                break;
        }
    }

    //call this after every tutorial is done
    private void NextStep()
    {
        if (tutorialStepsDone[tutorialStage])
        {
            tutorialStage++;
            StartCoroutine(WaitTime(tutorialStage));
        }
    }

    private void SwipeHorizontal()
    {
        //show player to swipe Horizontally

        if (!tutorialStepsDone[0])
        {
            tutorialStepsDone[0] = true;
            animatedPointer.SetActive(true);
            pointerAniamtor.SetTrigger(pointerAnimatorSteps[1]);
        }
    }

    private void OrientationButtonTut()
    {
        //point at Orientation Button

        if (!tutorialStepsDone[1])
        {
            //OrientationButtonPressEvent = new GameEvent();
            //var myListener = gameObject.AddComponent<GameEventListener>();
            //myListener.Event = OrientationButtonPressEvent;
            //myListener.Response.AddListener(new UnityAction(NextStep));
            OrientationButton.onClick.AddListener(new UnityAction(OrientationButtonPressEvent.Raise));

            pointerAniamtor.SetTrigger(pointerAnimatorSteps[0]);
            animatedPointer.SetActive(false);
            nonAnimatedPointer.SetActive(true);

            var rectt = OrientationButton.GetComponent<RectTransform>();

            nonAnimatedPointer.transform.SetParent(rectt);
            nonAnimatedPointer.transform.localPosition = new Vector3(-rectt.sizeDelta.x, 0, 0);
            nonAnimatedPointer.transform.localRotation = Quaternion.Euler(0, 0, -145);

            tutorialStepsDone[1] = true;
        }
    }

    private void IntroduceZoom()
    {
        //show pinch

    }

    private void OrientationWorkerTouch()
    {
        //show the player that they can touch workers to get their profiles

        if (!tutorialStepsDone[2])
        {
            //point at all workers
            workersInOrientation = workersList.WorkersInOrientation();

            if (workersInOrientation == null)
            {
                Debug.LogError("No workers are in orientation?.. this is a mistake");
            }
            else
            {
                nonAnimatedPointer.transform.SetParent(parentToPointers);
                //nonAnimatedPointer.GetComponentInChildren<Animator>().SetTrigger("Off");
                StartCoroutine(CycleThroughWorkers());

                tutorialStepsDone[2] = true;
            }
        }
    }

    IEnumerator CycleThroughWorkers()
    {
        int i = 0;

        while (true)
        {
            var workerPos = workersInOrientation[i].transform.position;
            Vector2 workerOnScreenPos = Camera.main.WorldToScreenPoint(workerPos);

            workerOnScreenPos.x += averageWorkerWidth;
            workerOnScreenPos.y += averageWorkerHeight;

            nonAnimatedPointer.transform.position = workerOnScreenPos;

            yield return new WaitForSeconds(1.5f);
            i = (i + 1) % (workersInOrientation.Count);
        }
    }

    private void SwipeVertical()
    {
        pointerAniamtor.SetTrigger(pointerAnimatorSteps[2]);
    }

    //events
    public void SwipedHorizontal()
    {
        if (tutorialStepsDone[0] && !tutorialStepsDone[1])
        {
            NextStep();
        }
    }
    public void PressedOrientationButton()
    {
        if (tutorialStepsDone[1] && !tutorialStepsDone[2])
        {
            NextStep();
        }
    }
    public void ClickedOnWorkerInOrientation()
    {
        if (tutorialStepsDone[2] && !tutorialStepsDone[3])
        {
            NextStep();
        }
    }
}
