using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Simple script to Handle the logic for the 'one sided door':
// 1. opening/closing of the door using animator
// 2. fade door when player is inside

// Info: Door Y axis is rotated, so Y is pointing in world up

public class Door : MonoBehaviour
{
    [SerializeField] Transform doorTransform;
    [SerializeField] float rotationAngle = 90.0f;
    [SerializeField] float animationTime = 1.0f;
    [SerializeField] bool isOpen = true;
    public bool IsOpen {  get { return isOpen; } }

    [Tooltip("Set this to true if you want the opposite flip")]
    [SerializeField] bool invertedRotation = false;
    [SerializeField] DoorMaterialFader doorFader;
    [SerializeField] HideTrigger hideTrigger;

    float openedAngle;
    float closedAngle;
    bool animationRunning = false;
    public bool IsAnimationRunning { get {  return animationRunning; } }

    public bool playerIsInside = false;

    float angleX;
    float angleZ;

    private void Start()
    {
        angleX = doorTransform.eulerAngles.x;
        angleZ = doorTransform.eulerAngles.z;

        // figure out worldspace Y rotation
        if (isOpen)
        {
            openedAngle = transform.eulerAngles.y;
            closedAngle = !invertedRotation ? openedAngle - rotationAngle : openedAngle + rotationAngle;
        }
        else
        {
            closedAngle = transform.eulerAngles.y;
            openedAngle = !invertedRotation ? closedAngle - rotationAngle : closedAngle + rotationAngle;
        }
    }

    public void PlayDoorAnimation()
    {
        StartCoroutine(PlayAnimation());
    }

    // rotate door and handle fade
    IEnumerator PlayAnimation()
    {
        if (animationRunning)
        {
            yield break;
        }

        // remember "on input if player is inside"
        playerIsInside = hideTrigger.playerIsInside;

        animationRunning = true;

        float timeElapsed = 0;

        float startRotation = doorTransform.eulerAngles.y;
        float newRotation = !isOpen ? openedAngle : closedAngle;

        // play door animation
        while (timeElapsed < animationTime)
        {
            float angle = Mathf.LerpAngle(startRotation, newRotation, timeElapsed / animationTime);
            doorTransform.eulerAngles = new Vector3(angleX, angle, angleZ);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        doorTransform.eulerAngles = new Vector3(angleX, newRotation, angleZ);

        // only allow door fade if player is inside
        if (playerIsInside)
        {
            doorFader.PlayDoorFade(IsOpen);

        }

        isOpen = !isOpen;

        animationRunning = false;
    }
}
