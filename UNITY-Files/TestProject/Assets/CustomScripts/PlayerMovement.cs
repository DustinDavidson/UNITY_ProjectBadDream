using UnityEngine;

// Make sure the player has a Rigidbody
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovements : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    [Header("Mouse Settings")]
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;

    private Rigidbody rb;
    private float xRotation = 0f;

    private float horizontalInput;
    private float verticalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Prevent player from tipping over
        rb.freezeRotation = true;
        // Lock cursor to center
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // --- Mouse Look ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // prevent flipping camera
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the player left/right
        transform.Rotate(Vector3.up * mouseX);

        // --- Input ---
        horizontalInput = Input.GetAxis("Horizontal"); // A/D keys
        verticalInput = Input.GetAxis("Vertical");     // W/S keys

        // --- Jump ---
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // --- Movement ---
        Vector3 move = transform.right * horizontalInput + transform.forward * verticalInput;
        Vector3 horizontalVelocity = move * moveSpeed;

        // Preserve vertical velocity (gravity/jumping)
        rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);
    }

    // --- Ground Check ---
    bool IsGrounded()
    {
        // Raycast down from the player
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
