using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    //Welcome player.
    //Introduce Swiping Left/Right (Horizontal) Rotating Floor
    public GameObjectField gameManagerField;
    private GameManager gManager;

    [Header("Input Manager")]
    [SerializeField]
    [Attributes.GreyOut]
    private InputManager gameInputManager;
    [SerializeField]
    [Attributes.GreyOut]
    private RaycastingByTouch cameraTap;

    [Header("Lists")]
    public WorkerList workersList;
    public EmptyMachinePlaceList listofEmptyMachines;
    public MachineList listOfMachines;

    [Header("Language")]
    public LanguageProfile lang;

    [Header("Pointer")]
    public Transform parentToPointers;
    public GameObject animatedPointer;
    public GameObject nonAnimatedPointer;
    public List<GameObject> multiplePointers;
    private Animator pointerAniamtor;
    public List<string> pointerAnimatorSteps;
    private bool movingPointerInCode;

    [Header("Tutorial Steps")]
    public int tutorialStage = 0;
    public float waitTimeBetweenTuts = 1.5f;
    [SerializeField]
    private bool[] tutorialStepsDone;

    [Tooltip("To add these events on the corresponding buttons")]
    [Header("Events")]
    public GameEvent OrientationButtonPressEvent;
    public GameEvent HireButtonPressEvent;
    public GameEvent StoreButtonPress;
    public GameEvent MapButtonPress;

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
    [SerializeField]
    [Attributes.GreyOut]
    private Button MapButtom;

    [Header("OrientationWorkers")]
    [SerializeField]
    float averageWorkerWidth = -15f; //x
    [SerializeField]
    float averageWorkerHeight = 50f; //y
    [SerializeField]
    [Attributes.GreyOut]
    List<Worker> workersInOrientation;
    Coroutine cycleThroughWorkersCoroutine;

    private void Start()
    {
        //All things in humanity
        pointerAniamtor = animatedPointer.GetComponent<Animator>();
        OrientationButton = FindObjectOfType<OrientationButtonIcon>().GetComponent<Button>();
        StoreButton = GameObject.Find("StoreButton").GetComponent<Button>();
        MapButtom = GameObject.Find("MapButton").GetComponent<Button>();
        gameInputManager = FindObjectOfType<InputManager>();
        cameraTap = FindObjectOfType<RaycastingByTouch>();
        gManager = gameManagerField.gameObjectReference.GetComponent<GameManager>();

        //setting up stuff
        StoreButton.interactable = false;
        MapButtom.interactable = false;
        OrientationButton.interactable = false;
        multiplePointers = new List<GameObject>();

        //hypothetical number of steps
        tutorialStepsDone = new bool[11];

        //starting the first step.. maybe change this later to let the save manager do it
        StartingPoint(tutorialStage);
    }

    public void StartingPoint(int tutorialStepSaved)
    {
        tutorialStage = tutorialStepSaved;
        for (int i = 0; i < tutorialStepSaved; i++)
        {
            tutorialStepsDone[i] = true;
        }

        //depending on the step, unlock all the buttons and turn them interactable

        StopAllCoroutines();
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
                PointAtHireButton();
                break;
            case 4:
                //go back to factory
                BackToFactoryButton();
                break;
            case 5:
                PointAtStoreButton();
                break;
            case 6:
                SwipeVertical();
                break;
            case 7:
                PointAtEmptyMachinePlace();
                break;
            case 8:
                PointAtMapButton();
                break;
            case 9:
                PointAtWorkerInMap();
                break;
            case 10:
                PointAtStoreForBlueMachine();
                break;
            default:
                Debug.LogWarning("Case " + i + " Not Implemeneted yet");
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
        //0
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
            OrientationButton.interactable = true;
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

    #region PointAtWorkersInOrientation
    //point at workers in orientation
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
                cycleThroughWorkersCoroutine = StartCoroutine(CycleThroughWorkers());

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
    #endregion

    //point at button of Hiring Worker
    private void PointAtHireButton()
    {
        if (!tutorialStepsDone[3])
        {
            HireButton = GameObject.Find("HireButton").GetComponent<Button>();
            var rectHireButton = HireButton.GetComponent<RectTransform>();
            nonAnimatedPointer.SetActive(true);

            nonAnimatedPointer.transform.SetParent(rectHireButton);
            nonAnimatedPointer.transform.localPosition = new Vector3(-rectHireButton.sizeDelta.x, rectHireButton.sizeDelta.y, 0);
            nonAnimatedPointer.transform.localRotation = Quaternion.Euler(0, 0, -145);

            tutorialStepsDone[3] = true;
        }
    }

    private void BackToFactoryButton()
    {
        //point at Orientation Button

        if (!tutorialStepsDone[4])
        {
            //OrientationButton.onClick.AddListener(new UnityAction(OrientationButtonPressEvent.Raise));

            pointerAniamtor.SetTrigger(pointerAnimatorSteps[0]);
            animatedPointer.SetActive(false);
            nonAnimatedPointer.SetActive(true);

            var rectt = OrientationButton.GetComponent<RectTransform>();

            nonAnimatedPointer.transform.SetParent(rectt);
            nonAnimatedPointer.transform.localPosition = new Vector3(-rectt.sizeDelta.x, 0, 0);
            nonAnimatedPointer.transform.localRotation = Quaternion.Euler(0, 0, -145);

            tutorialStepsDone[4] = true;
        }
    }

    private void PointAtStoreButton()
    {
        if (!tutorialStepsDone[5] || !tutorialStepsDone[11])
        {
            StoreButton.interactable = true;

            var rectHireButton = StoreButton.GetComponent<RectTransform>();
            StoreButton.onClick.AddListener(new UnityAction(StoreButtonPress.Raise));

            nonAnimatedPointer.SetActive(true);
            nonAnimatedPointer.transform.SetParent(rectHireButton);
            nonAnimatedPointer.transform.localPosition = new Vector3(-rectHireButton.sizeDelta.x, rectHireButton.sizeDelta.y, 0);
            nonAnimatedPointer.transform.localRotation = Quaternion.Euler(0, 0, -145);

            tutorialStepsDone[5] = true;
        }
    }

    private void SwipeVertical()
    {
        if (!tutorialStepsDone[7])
        {
            animatedPointer.SetActive(true);
            pointerAniamtor.SetTrigger(pointerAnimatorSteps[2]);

            tutorialStepsDone[7] = true;
        }
    }

    private void PointAtEmptyMachinePlace()
    {
        if (!tutorialStepsDone[8])
        {
            animatedPointer.SetActive(false);
            nonAnimatedPointer.SetActive(true);

            var emptyMachinePlace = listofEmptyMachines.Items[4];
            //raycast from place to camera, if it is not in view, swipe to find it

            var requiredPos = emptyMachinePlace.transform.position;
            Vector2 onScreenPos = Camera.main.WorldToScreenPoint(requiredPos);

            onScreenPos.x += averageWorkerWidth;
            onScreenPos.y += averageWorkerHeight;

            nonAnimatedPointer.transform.position = onScreenPos;

            tutorialStepsDone[8] = true;
        }
    }

    private void PointAtMapButton()
    {
        if (!tutorialStepsDone[9])
        {
            animatedPointer.SetActive(false);
            MapButtom.interactable = true;

            var rectHireButton = MapButtom.GetComponent<RectTransform>();
            MapButtom.onClick.AddListener(new UnityAction(MapButtonPress.Raise));

            nonAnimatedPointer.SetActive(true);
            nonAnimatedPointer.transform.SetParent(rectHireButton);
            nonAnimatedPointer.transform.localPosition = new Vector3(-rectHireButton.sizeDelta.x, rectHireButton.sizeDelta.y, 0);
            nonAnimatedPointer.transform.localRotation = Quaternion.Euler(0, 0, -145);

            tutorialStepsDone[5] = true;

            tutorialStepsDone[9] = true;
        }
    }

    private void PointAtWorkerInMap()
    {
        if (!tutorialStepsDone[10])
        {
            //points at a worker in the map and tells the player to drag the worker to the machine
            UIMap map = FindObjectOfType<UIMap>();
            UIFloor floor = map.GetComponentInChildren<UIFloor>();

            if (map != null)
            {
                var workerIcon = map.iconsList.Items[0];
                var workerTransform = workerIcon.GetComponent<RectTransform>();

                nonAnimatedPointer.SetActive(true);
                nonAnimatedPointer.GetComponentInChildren<Animator>().SetTrigger("Off");

                nonAnimatedPointer.transform.SetParent(workerTransform);
                nonAnimatedPointer.transform.localPosition = new Vector3(0, 0, 0);
                nonAnimatedPointer.transform.localRotation = Quaternion.Euler(0, 0, 145);
                nonAnimatedPointer.transform.SetParent(parentToPointers);

                UIMapDraggingContainer machinePosition = null;
                for (int i = 0; i < floor.rooms.Length; i++)
                {
                    int j = 0;
                    while (j < floor.rooms[i].Machines.Length && machinePosition == null)
                    {
                        if (floor.rooms[i].Machines[j].machineReference != null)
                            machinePosition = floor.rooms[i].Machines[j];
                        else
                            j++;
                    }
                }

                if (machinePosition == null)
                {
                    Debug.LogError("map machine is null");
                    return;
                }

                //start coroutine(machine position as given)
                StartCoroutine(pointFromWorkerToMachine(machinePosition));

                tutorialStepsDone[10] = true;
            }
            else
            {
                Debug.LogWarning("UIMap is null.\n couldn't find an object of type <UIMap> in Scene");
            }
        }
    }
    private IEnumerator pointFromWorkerToMachine(UIMapDraggingContainer machine)
    {
        Vector2 originalPointerPos = nonAnimatedPointer.transform.position;

        nonAnimatedPointer.transform.SetParent(machine.transform);
        nonAnimatedPointer.transform.localPosition = new Vector3(0, 0, 0);
        Vector2 machineRealPos = nonAnimatedPointer.transform.position;
        nonAnimatedPointer.transform.SetParent(parentToPointers);

        nonAnimatedPointer.transform.position = originalPointerPos;

        float t = 0;

        movingPointerInCode = true;
        while (movingPointerInCode && machine.workerImage == null)
        {

            if (t <= 1f)
            {
                t += Time.deltaTime;
                nonAnimatedPointer.transform.position = Vector2.Lerp(nonAnimatedPointer.transform.position, machineRealPos, t/2f);
            }
            else
            {
                t = 0;
                nonAnimatedPointer.transform.position = originalPointerPos;
            }

            yield return new WaitForEndOfFrame();
        }

        WorkerMovedInMap();
    }

    private void PointAtStoreForBlueMachine()
    {
        if (!tutorialStepsDone[11])
        {
            //making the machine finish first cycle NOW
            Machine machine = listOfMachines.Items[0];
            machine.FinishCycleNow();
            PointAtStoreButton();

            tutorialStepsDone[11] = true;
        }
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
            StopCoroutine(cycleThroughWorkersCoroutine);
            nonAnimatedPointer.SetActive(false);
            NextStep();
        }
    }
    public void PressedHireWorkerButton()
    {
        if (tutorialStepsDone[3] && !tutorialStepsDone[4])
        {
            NextStep();
        }
    }
    //IEnumerator WaitToRaiseEvent()
    //{
    //    while (tutorialStepsDone[3] && !tutorialStepsDone[4])
    //    {
    //        yield return new WaitForSeconds(1f);
    //    }
    //    switchCameraView.Raise();
    //}
    public void PressedBackFromOrientationButton()
    {
        if (tutorialStepsDone[4] && !tutorialStepsDone[5])
        {
            NextStep();
        }
    }
    public void PressedStoreButton()
    {
        if (tutorialStepsDone[5] && !tutorialStepsDone[6])
        {
            nonAnimatedPointer.SetActive(false);
            tutorialStepsDone[6] = true;
        }
        else if (tutorialStepsDone[11])
        {
            nonAnimatedPointer.SetActive(false);
            tutorialStepsDone[6] = true;
            //var store = FindObjectOfType<StorePanel>();
            //Debug.Log("Store name: "+store.name);
        }
    }
    public void BoughtMachineWaitingToPlaceIt()
    {
        if (tutorialStepsDone[6] && !tutorialStepsDone[7])
        {
            var floor = FindObjectOfType<Floor>();
            floor.transform.localRotation = Quaternion.Euler(0, 75, 0);
            NextStep();
        }
    }
    public void SwipeVerticalTutorialDone()
    {
        if (tutorialStepsDone[7] && !tutorialStepsDone[8])
        {
            NextStep();
        }
    }
    public void PlacedMachineInFloor()
    {
        if (tutorialStepsDone[8] && !tutorialStepsDone[9])
        {
            NextStep();
        }
    }
    public void PressedMapButton()
    {
        if (tutorialStepsDone[9] && !tutorialStepsDone[10])
        {
            nonAnimatedPointer.SetActive(false);
            NextStep();
        }
    }
    private void WorkerMovedInMap()
    {
        if (tutorialStepsDone[10] && !tutorialStepsDone[11])
        {
            UIMap map = FindObjectOfType<UIMap>();
            map.gameObject.SetActive(false);
            movingPointerInCode = false;
            nonAnimatedPointer.SetActive(false);
            NextStep();
        }
    }
}
