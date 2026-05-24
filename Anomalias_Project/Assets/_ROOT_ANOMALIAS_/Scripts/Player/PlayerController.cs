using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;

    public float acceleration = 12f;
    public float deceleration = 14f;

    public float mouseSensitivity = 0.1f;

    public float maxSlopeAngle = 45f;

    public Transform cameraHolder;

    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    public float headBobSpeed = 14f;
    public float headBobAmount = 0.05f;

    private Rigidbody rb;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private Vector3 currentVelocity;

    private float xRotation;

    private bool isGrounded;

    private Vector3 cameraStartPos;

    private float bobTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;

        cameraStartPos = cameraHolder.localPosition;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        GroundCheck();

        Look();

        HeadBob();
    }

    void FixedUpdate()
    {
        Move();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundDistance,
            groundMask
        );
    }

    void Move()
    {
        Vector3 moveDirection =
            transform.right * moveInput.x +
            transform.forward * moveInput.y;

        moveDirection.Normalize();

        Vector3 targetVelocity =
            moveDirection * moveSpeed;

        Vector3 velocity =
            rb.linearVelocity;

        Vector3 horizontalVelocity =
            new Vector3(
                velocity.x,
                0f,
                velocity.z
            );

        float smooth =
            moveDirection.magnitude > 0
            ? acceleration
            : deceleration;

        horizontalVelocity =
            Vector3.Lerp(
                horizontalVelocity,
                targetVelocity,
                smooth * Time.fixedDeltaTime
            );

        rb.linearVelocity =
            new Vector3(
                horizontalVelocity.x,
                rb.linearVelocity.y,
                horizontalVelocity.z
            );
    }

    void Look()
    {
        float mouseX =
            lookInput.x * mouseSensitivity;

        float mouseY =
            lookInput.y * mouseSensitivity;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(
            xRotation,
            -90f,
            90f
        );

        cameraHolder.localRotation =
            Quaternion.Euler(
                xRotation,
                0f,
                0f
            );

        transform.Rotate(
            Vector3.up * mouseX
        );
    }

    void HeadBob()
    {
        Vector3 horizontalVelocity =
            new Vector3(
                rb.linearVelocity.x,
                0f,
                rb.linearVelocity.z
            );

        if (
            horizontalVelocity.magnitude > 0.1f &&
            isGrounded
        )
        {
            bobTimer +=
                Time.deltaTime * headBobSpeed;

            Vector3 targetPos =
                cameraStartPos;

            targetPos.y +=
                Mathf.Sin(bobTimer) *
                headBobAmount;

            cameraHolder.localPosition =
                targetPos;
        }
        else
        {
            bobTimer = 0f;

            cameraHolder.localPosition =
                Vector3.Lerp(
                    cameraHolder.localPosition,
                    cameraStartPos,
                    8f * Time.deltaTime
                );
        }
    }
}