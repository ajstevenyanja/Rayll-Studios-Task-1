using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// useful when you want to call multiple things on start

public class OnStartEvent : MonoBehaviour
{
    [SerializeField] UnityEvent OnStart;

    private void Start()
    {
        OnStart.Invoke();
    }
}
