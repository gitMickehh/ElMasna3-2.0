using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnableDisableEvents : MonoBehaviour
{
    public UnityEvent onEnableEvents;
    public UnityEvent onDisableEvents;

    private void OnEnable()
    {
        onEnableEvents.Invoke();
    }

    private void OnDisable()
    {
        onDisableEvents.Invoke();
    }
}
