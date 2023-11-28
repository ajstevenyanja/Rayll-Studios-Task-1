using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// toggles hidden flag of the door
// based on player collision with this trigger
// addon: toggles voice activity bar

public class HideTrigger : MonoBehaviour
{
    [SerializeField] Door door;
    [SerializeField] GameObject voiceActivityBar;
    public bool playerIsInside;

    private void Start()
    {
        if (door == null)
        {
            door = GameObject.FindObjectOfType<Door>();
        }
        voiceActivityBar.SetActive(false);
    }

    private void Update()
    {
        if (playerIsInside && !door.IsOpen)
        {
            voiceActivityBar.SetActive(true);
        }
        else
        {
            voiceActivityBar.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            door.playerIsInside = true;
            playerIsInside = true;
            voiceActivityBar.SetActive(true);
        }
    }

/*    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsInside = true;
        }
    }*/

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsInside = false;
            voiceActivityBar.SetActive(false);
        }
    }
}
