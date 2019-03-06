using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TestModalWindow : MonoBehaviour
{
    private ModalPanel modalPanel;
    private DisplayManager displayManager;

    private UnityAction myYesAction;
    private UnityAction myNoAction;
    private UnityAction myCancelAction;

    void Awake()
    {
        modalPanel = ModalPanel.Instance();
        displayManager = DisplayManager.Instance();

        myYesAction = new UnityAction(TestYesFunction);
        myNoAction = new UnityAction(TestNoFunction);
        myCancelAction = new UnityAction(TestCancelFunction);
    }

    public void TestYNC()
    {
        modalPanel.Choice("Would you like a poke in the eye? \nHow about with a sharp stick?", myYesAction, myNoAction, myCancelAction);
    }
    void TestYesFunction()
    {
        displayManager.DisplayMessage("Heck yeah! Yup!");
    }

    void TestNoFunction()
    {
        displayManager.DisplayMessage("No Way!");
    }

    void TestCancelFunction()
    {
        displayManager.DisplayMessage("I give up");
    }
}
