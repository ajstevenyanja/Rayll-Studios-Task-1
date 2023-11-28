using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Add your functions or logic in the events section of this script (in the inspector)

[RequireComponent(typeof(Collider))]
public class OnTriggerEvents : MonoBehaviour
{
    [SerializeField] UnityEvent OnTriggerEntered;
    [SerializeField] UnityEvent OnTriggerStayed;
    [SerializeField] UnityEvent OnTriggerExited;

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEntered.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        OnTriggerStayed.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExited.Invoke();
    }
}
