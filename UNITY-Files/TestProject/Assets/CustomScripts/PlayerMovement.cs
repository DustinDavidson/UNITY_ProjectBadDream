using UnityEngine;

// Make sure the player has a Rigidbody
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public Transform handPoint; // Point where the player holds items
    public Vector3 screenOffset = new Vector3(0.3f,-0.3f,0.5f);
    public float handReach = 1.5f; // Distance the player can reach with their hand
    public float moveSpeed = 2f;
    public float sprintMultiplier = 1.5f;
    public float jumpForce = 4f;
    GameObject heldItem; // Currently held item

    [Header("Mouse Settings")]
    public Transform cameraTransform; // Reference to the player's camera
    public float mouseSensitivity = 2f;

    public GameManager gameManager; // Reference to GameManager for handling pause

    private Rigidbody rb;
    private Player player; // Reference to Player class
    private Door door; // Reference to Door class
    private float xRotation = 0f; //    

    private float horizontalInput;
    private float verticalInput;

    private float originalSpeed;

    private float currentSpeed;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponent<Player>();

        currentSpeed = moveSpeed;
        originalSpeed = moveSpeed;
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

        if (Input.GetKeyDown(KeyCode.G))
        {
            DropObject();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.PauseGame();
        }
           
        // --- Mouse Look ---
        if(Time.timeScale != 0f)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

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

            // --- Sprint ---
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = sprintMultiplier * originalSpeed;
            }
            else
            {
                moveSpeed = originalSpeed;
            }
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

    void LateUpdate()
    {
        // lock position to camera
        handPoint.position = cameraTransform.position + cameraTransform.right * screenOffset.x + cameraTransform.up * screenOffset.y + cameraTransform.forward * screenOffset.z;

        // copy rotation
        handPoint.rotation = Quaternion.LookRotation(cameraTransform.forward);
    }


    // --- Item Pickup Logic ---
    void TryPickup()
    {
        Debug.Log("E key pressed - attempting pickup");
        
        // Cast multiple rays in a cone pattern 
        float coneAngle = 5f;       // total cone width in degrees
        int numRays = 4;             // how many rays across the cone
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
                Quaternion rot = Quaternion.AngleAxis(yaw, Vector3.up) * 
                                 Quaternion.AngleAxis(pitch, Camera.main.transform.right);
                
                // Calculate the ray direction
                Vector3 rayDir = rot * Camera.main.transform.forward;

                // Cast the ray
                if (Physics.Raycast(Camera.main.transform.position, rayDir, out hit, handReach))
                { 
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
                            return;

                        case "KEY":
                            Debug.Log("Key object detected. Picking up key.");
                            if (player != null)
                            {
                                KeyObject keyComp = hit.collider.GetComponent<KeyObject>();
                                if (keyComp != null)
                                {
                                    player.AddItem(keyComp.itemData); // pass the ScriptableObject
                                }
                                else
                                {
                                    Debug.LogWarning("Key prefab is missing the KeyObject component!");
                                }
                            }
                            hit.collider.gameObject.SetActive(false); // hide the key in the world
                            return;


                        case "DOOR":
                            Door hitDoor = hit.collider.GetComponent<Door>();
                            
                            // If not found, check the parent
                            if (hitDoor == null)
                            {
                                hitDoor = hit.collider.GetComponentInParent<Door>();
                            }
                            
                            if (hitDoor != null)
                            {
                                hitDoor.ToggleDoor();
                            }
                            else
                            {
                                Debug.Log("The object '" + hit.collider.gameObject.name + "' doesn't have a Door component!");
                            }
                            return;

                        case "LOCK":
                            Door doorComponent = hit.collider.GetComponent<Door>();
                            
                            // If not found, check the parent
                            if (doorComponent == null)
                            {
                                doorComponent = hit.collider.GetComponentInParent<Door>();
                            }
                            
                            if (doorComponent != null)
                            {
                                doorComponent.LockToggle(player);
                            }
                            else
                            {
                                Debug.Log("The object '" + hit.collider.gameObject.name + "' doesn't have a Door component!");
                            }
                            return;

                    default:
                        break;
                    }
                }
            }
        }
    }
        
    // --- Pick Up Object ---
   void PickupObject(GameObject obj)
    {
        heldItem = obj;
        
        Rigidbody objRb = obj.GetComponent<Rigidbody>();
        if (objRb != null)
        {
            objRb.isKinematic = true;
        }

        Collider col = obj.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
        
        obj.transform.SetParent(handPoint);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.Euler(0, -95, 0);
        
        // Notify the flashlight it's been picked up
        FlashLight flashlight = obj.GetComponentInChildren<FlashLight>();
        if (flashlight != null)
        {
            Debug.Log("✅ FlashLight component FOUND on " + obj.name);  // ⭐ ADD THIS
            flashlight.OnPickedUp();
            Debug.Log("✅ OnPickedUp() was CALLED");  // ⭐ ADD THIS
        }
        else
        {
            Debug.Log("❌ NO FlashLight component on " + obj.name);  // ⭐ ADD THIS
        }
        
        Debug.Log("Picked up " + obj.name);
    }

    void DropObject()
    {
        if (heldItem == null)
        {
            Debug.Log("No item to drop.");
            return;
        }

        // Notify the flashlight it's been dropped
        FlashLight flashlight = heldItem.GetComponentInChildren<FlashLight>();
        if (flashlight != null)
        {
            flashlight.OnDropped();
        }

        Rigidbody objRb = heldItem.GetComponent<Rigidbody>();
        Collider col = heldItem.GetComponent<Collider>();
        heldItem.transform.position = handPoint.position + handPoint.forward * 0.5f + Vector3.up * 0.2f;

        heldItem.transform.SetParent(null);
        if (objRb != null)
        {
            objRb.isKinematic = false;
        }

        if (col != null)
        {
            col.enabled = true;
        }

        
        heldItem = null;
        
        Debug.Log("Dropped item.");
    }

    // --- Ground Check ---
    bool IsGrounded()
    {
        // Raycast down from the player
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
