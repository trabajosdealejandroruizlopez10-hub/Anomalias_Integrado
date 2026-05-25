using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;

    public float acceleration = 12f;
    public float deceleration = 14f;

    [Header("Mouse")]
    public float mouseSensitivity = 2f;

    public Transform cameraHolder;

    [Header("Ground Check")]
    public Transform groundCheck;

    public float groundDistance = 0.3f;

    public LayerMask groundMask;

    [Header("Head Bob")]
    public float headBobSpeed = 14f;

    public float headBobAmount = 0.05f;

    private Rigidbody rb;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private float xRotation;

    private bool isGrounded;

    private Vector3 cameraStartPos;

    private float bobTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.interpolation =
            RigidbodyInterpolation.Interpolate;

        rb.collisionDetectionMode =
            CollisionDetectionMode.Continuous;

        rb.constraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;

        cameraStartPos =
            cameraHolder.localPosition;

        Cursor.lockState =
            CursorLockMode.Locked;

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

    public void OnMove(
        InputAction.CallbackContext context
    )
    {
        moveInput =
            context.ReadValue<Vector2>();
    }

    public void OnLook(
        InputAction.CallbackContext context
    )
    {
        lookInput =
            context.ReadValue<Vector2>();
    }

    void GroundCheck()
    {
        isGrounded =
            Physics.CheckSphere(
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
            lookInput.x *
            mouseSensitivity;

        float mouseY =
            lookInput.y *
            mouseSensitivity;

        xRotation -= mouseY;

        xRotation =
            Mathf.Clamp(
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

        float yRotation =
            transform.eulerAngles.y +
            mouseX;

        transform.rotation =
            Quaternion.Euler(
                0f,
                yRotation,
                0f
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
                Time.deltaTime *
                headBobSpeed;

            Vector3 targetPos =
                cameraStartPos;

            targetPos.y +=
                Mathf.Sin(
                    bobTimer
                ) * headBobAmount;

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

    public void Teleport(
        Vector3 position,
        Quaternion rotation
    )
    {
        rb.linearVelocity =
            Vector3.zero;

        rb.angularVelocity =
            Vector3.zero;

        transform.position =
            position;

        transform.rotation =
            Quaternion.Euler(
                0f,
                rotation.eulerAngles.y,
                0f
            );
    }
}