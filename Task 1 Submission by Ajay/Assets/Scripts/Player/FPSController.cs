using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPSRigidbodyController : MonoBehaviour
{
    [Header("Camera settings")]
    [SerializeField] Camera playerCamera;
    [SerializeField] Vector2 cameraVerticalLimit = new Vector2(-60, 45);
    [SerializeField] private float sensitivity = 2.0f;
    [SerializeField] private float defaultFOV = 60.0f;
    [SerializeField] private float zoomFOV = 45.0f;
    [SerializeField] float zoomSmoothTime = 0.1f;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float runSpeed = 6.0f;
    [SerializeField] private float crouchSpeed = 1.5f;
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 2.0f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] bool isGrounded;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    public bool disableInputs = false;

    private Rigidbody rb;
    private bool isCrouching = false;
    private bool isCrouchingCoroutineRunning = false;
    private float crouchSmoothTime = 0.2f;

    private float zoomVelocity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();
        ConvertVariablesForDeltaTime();
    }

    private void Update()
    {
        if (disableInputs)
            return;

        HandleMovement();
        HandleMouseLook();
        HandleCrouch();
        HandleJump();
        HandleZoom();
    }

    private void HandleMovement()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        speed = isCrouching ? crouchSpeed : speed;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.TransformDirection(new Vector3(horizontal, 0, vertical));
        rb.velocity = new Vector3(moveDirection.x * speed * Time.deltaTime, rb.velocity.y,
            moveDirection.z * speed * Time.deltaTime);
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = -Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        float currentRotationX = playerCamera.transform.localEulerAngles.x + mouseY;

        currentRotationX = ClampRotation(currentRotationX, cameraVerticalLimit.x, cameraVerticalLimit.y);

        playerCamera.transform.localEulerAngles = new Vector3(currentRotationX, 0, 0);
    }

    private void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isCrouchingCoroutineRunning)
        {
            StartCoroutine(CrouchStandToggle());
        }
    }

    private void HandleJump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            if (!isCrouching)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Acceleration);
            }
        }
    }

    private void HandleZoom()
    {
        float targetFOV = Input.GetMouseButton(1) ? zoomFOV : defaultFOV;

        // Smoothly lerp the field of view
        playerCamera.fieldOfView = Mathf.SmoothDamp(playerCamera.fieldOfView, targetFOV, ref zoomVelocity, zoomSmoothTime);
    }

    private float ClampRotation(float angle, float min, float max)
    {
        if (angle < 180)
        {
            return Mathf.Clamp(angle, min, max);
        }
        else
        {
            float normalizedAngle = angle - 360;
            return Mathf.Clamp(normalizedAngle, min, max) + 360;
        }
    }

    IEnumerator CrouchStandToggle()
    {
        isCrouchingCoroutineRunning = true;

        isCrouching = !isCrouching;

        float targetHeight = isCrouching ? crouchHeight : standingHeight;

        // Smoothly lerp the height
        float elapsedTime = 0f;
        float initialHeight = rb.transform.localScale.y;

        while (elapsedTime < crouchSmoothTime)
        {
            float newHeight = Mathf.Lerp(initialHeight, targetHeight, elapsedTime / crouchSmoothTime);
            rb.transform.localScale = new Vector3(rb.transform.localScale.x, newHeight, rb.transform.localScale.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.transform.localScale = new Vector3(rb.transform.localScale.x, targetHeight, rb.transform.localScale.z);
        isCrouchingCoroutineRunning = false;
    }

    private void ConvertVariablesForDeltaTime()
    {
        sensitivity *= 60.0f;
        walkSpeed *= 60.0f;
        runSpeed *= 60.0f;
        crouchSpeed *= 60.0f;
        jumpForce *= 60.0f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
    }
}
