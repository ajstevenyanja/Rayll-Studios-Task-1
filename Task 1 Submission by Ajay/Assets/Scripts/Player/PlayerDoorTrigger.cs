using UnityEngine;
using UnityEngine.UI;

// Simple script that allows player interactions

// 'This trigger detects collision against Doors colliders'
// for extended use in the future like crosshair based interactions we can use ray cast with layer mask

public class PlayerDoorTrigger : MonoBehaviour
{
    [SerializeField] bool isColliding = false;
    [SerializeField] KeyCode interactMouseButton = KeyCode.Mouse0;
    [SerializeField] KeyCode interactKey = KeyCode.E;

    [SerializeField] Image crosshairImage;
    [SerializeField] Sprite onHoverCrosshair;
    [SerializeField] Sprite normalCrosshair;

    Vector3 triggerScale;       // for GUI visualization
    Door currentDoor;

    private void Start()
    {
        triggerScale = transform.localScale;
        crosshairImage.sprite = normalCrosshair;
    }

    private void Update()
    {
        DrawDebugCube();

        if (Input.GetKeyDown(interactMouseButton) || Input.GetKeyDown(interactKey))
        {
            if (!isColliding) { return; }

            currentDoor.PlayDoorAnimation();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            isColliding = true;
            ChangeCrosshair(onHoverCrosshair);
            currentDoor = other.gameObject.GetComponent<Door>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        /*if (other.gameObject.CompareTag("Door"))
        {
            isColliding = true;
            ChangeCrosshair(onHoverCrosshair);
            currentDoor = other.gameObject.GetComponent<Door>();
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            isColliding = false;
            ChangeCrosshair(normalCrosshair);
            currentDoor = null;
        }
    }

    // debug draw wire cube to show collision
    void DrawDebugCube()
    {
        Vector3 center = transform.position;
        Quaternion rotation = transform.rotation;

        float halfX = triggerScale.x * 0.5f;
        float halfY = triggerScale.y * 0.5f;
        float halfZ = triggerScale.z * 0.5f;

        // Define the local coordinates of the corners of the rotated cube
        Vector3[] localCorners = new Vector3[]
        {
            new Vector3(halfX, halfY, halfZ),
            new Vector3(halfX, halfY, -halfZ),
            new Vector3(-halfX, halfY, -halfZ),
            new Vector3(-halfX, halfY, halfZ),
            new Vector3(halfX, -halfY, halfZ),
            new Vector3(halfX, -halfY, -halfZ),
            new Vector3(-halfX, -halfY, -halfZ),
            new Vector3(-halfX, -halfY, halfZ)
        };

        // Transform local coordinates to world coordinates
        Vector3[] corners = new Vector3[8];
        for (int i = 0; i < 8; i++)
        {
            corners[i] = center + rotation * localCorners[i];
        }

        // Draw lines between the corners
        for (int i = 0; i < 4; i++)
        {
            Debug.DrawLine(corners[i], corners[(i + 1) % 4], isColliding ? Color.green : Color.red);
            Debug.DrawLine(corners[i + 4], corners[(i + 1) % 4 + 4], isColliding ? Color.green : Color.red);
            Debug.DrawLine(corners[i], corners[i + 4], isColliding ? Color.green : Color.red);
        }
    }

    void ChangeCrosshair(Sprite inputSprite)
    {
        crosshairImage.sprite = inputSprite;
    }
}


/*    private void CheckForInteractions()
    {
        RaycastHit[] hits = Physics.BoxCastAll(interactorOrigin.position, Vector3.one * boxSize / 2, interactorOrigin.forward, Quaternion.LookRotation(interactorOrigin.forward), Mathf.Infinity, interactionLayers, QueryTriggerInteraction.Ignore);

        foreach (RaycastHit hit in hits)
        {
            Transform hitObject = hit.transform;

            Debug.Log(hit.transform.gameObject.name + " was hit");

            // Door check
            if (hitObject.CompareTag("Door"))
            {
                hitObject.GetComponent<Door>().PlayDoorAnimation();
            }
        }
    }
*/
