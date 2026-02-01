using UnityEngine;

// Make sure the player has a Rigidbody
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public Transform handPoint; // Point where the player holds items
    public float handReach = 3f; // Distance the player can reach with their hand
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    GameObject heldItem;

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
        // --- Item Pickup ---
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickup();
        }
           
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

    // --- Item Pickup Logic ---
    // --- Item Pickup Logic ---
    void TryPickup()
    {
        Debug.Log("E key pressed - attempting pickup");
        
        if (heldItem != null)
        {
            Debug.Log("Already holding an item");
            return; // already holding an item
        }

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, handReach))
        {
            Debug.Log("Raycast hit: " + hit.collider.gameObject.name + " at distance " + hit.distance);
            Debug.Log("Object tag: " + hit.collider.tag);
            
            if (hit.collider.CompareTag("PickUp"))
            {
                Debug.Log("Tag matched! Picking up object");
                PickupObject(hit.collider.gameObject);
            }
            else
            {
                Debug.Log("Tag doesn't match. Expected 'PickUp', got '" + hit.collider.tag + "'");
            }
        }
        else
        {
            Debug.Log("Raycast didn't hit anything within range");
        }
    }

    // --- Pick Up Object ---
    void PickupObject(GameObject obj)
    {
        heldItem = obj;

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // disable physics
        }
        obj.transform.SetParent(handPoint);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        Debug.Log("Picked up " + obj.name);
    }

    // --- Ground Check ---
    bool IsGrounded()
    {
        // Raycast down from the player
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
