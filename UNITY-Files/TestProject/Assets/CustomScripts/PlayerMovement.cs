using Unity.VisualScripting;
using UnityEngine;

// Make sure the player has a Rigidbody
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public Transform handPoint; // Point where the player holds items
    public float handReach = 2.5f; // Distance the player can reach with their hand
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.5f;
    public float jumpForce = 4f;
    GameObject heldItem; // Currently held item

    [Header("Mouse Settings")]
    public Transform cameraTransform; // Reference to the player's camera
    public float mouseSensitivity = 2f;

    private Rigidbody rb;
    private Player player; // Reference to Player class
    private Door door; // Reference to Door class
    private float xRotation = 0f; //    

    private float horizontalInput;
    private float verticalInput;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
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
    void TryPickup()
    {
        Debug.Log("E key pressed - attempting pickup");
        
        // Cast multiple rays in a cone pattern 
        float coneAngle = 30f;       // total cone width in degrees
        int numRays = 5;             // how many rays across the cone
        float angleStep = coneAngle / (numRays - 1);
        float startAngle = -coneAngle / 2f; // start at left edge of cone

        RaycastHit hit;

        for (int i = 0; i < numRays; i++)
        {
            float yaw = startAngle + i * angleStep;   // horizontal rotation
            for (int j = 0; j < numRays; j++)
            {
                float pitch = startAngle + j * angleStep; // vertical rotation

                // Combine rotations into a single direction
                Quaternion rot = Quaternion.AngleAxis(yaw, Vector3.up) * Quaternion.AngleAxis(pitch, Camera.main.transform.right);
                Vector3 rayDir = rot * Camera.main.transform.forward;

                // Draw the ray so you can see it in the Scene view
                Debug.DrawRay(Camera.main.transform.position, rayDir * handReach, Color.red, 0.1f);

                // Cast the ray
                if (Physics.Raycast(Camera.main.transform.position, rayDir, out hit, handReach))
                { 
                    Debug.Log("Raycast hit: " + hit.collider.gameObject.name + " at distance " + hit.distance);
                    Debug.Log("Object tag: " + hit.collider.tag);

                    string tag = hit.collider.tag;
                    switch (tag)
                    {
                        case "PickUp":
                            if (heldItem != null)
                            {
                                Debug.Log("Already holding an item");
                                return;
                            }
                            Debug.Log("Tag matched! Picking up object");
                            PickupObject(hit.collider.gameObject);
                            break;

                        case "KEY":
                            Debug.Log("Key object detected. Picking up key.");
                            if (player != null)
                            {
                                player.AddItem(hit.collider.gameObject.name);
                            }
                            hit.collider.gameObject.SetActive(false);
                            break;

                        case "DOOR":
                            // Get the Door component from the object the ray hit
                            Door hitDoor = hit.collider.GetComponent<Door>();
                            if (hitDoor != null)
                            {
                                hitDoor.TryOpenDoor(player);
                            }
                            else
                            {
                                Debug.Log("The object hit doesn't have a Door component!");
                            }
                            break;


                    default:
                        Debug.Log("Tag doesn't match. Expected 'PickUp', or 'KEY' got '" + tag + "'");
                        break;
                }
            }
                else
                {
                    Debug.Log("Raycast didn't hit anything within range");
                }
                }
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
        obj.transform.localRotation = Quaternion.Euler(0, -90, 0);
        Debug.Log("Picked up " + obj.name);
    }

    // --- Ground Check ---
    bool IsGrounded()
    {
        // Raycast down from the player
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
