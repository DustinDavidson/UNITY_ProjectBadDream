using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;

    public float forwardDistance = 3f;

    public float rayDistance = 3f;  

    // Number of rays to cast in the navigation arc
    public int numRays = 5;

    public int forwardRays = 3;

    public float rotateSpeed = 45f;

    public float rotationArc = 150f;

    public float forwardArc = 45f;

    private float backupTimer = 0f;

    // Angle between each ray, calculated from the total arc (150 degrees) divided by the number of gaps between rays
    private float rayDegree;

    private float forwardDegree;

    // Tracks vertical speed, accumulates over time when airborne to simulate gravity
    private float verticalVelocity;

    private CollisionFlags flags;

    private CharacterController character;
    private Player player; // Will be used to spot the player later on

    void Start()
    {
        character = GetComponent<CharacterController>();

        // Divide the 150 degree arc evenly between rays
        // Using numRays - 1 so the first and last rays land exactly on the arc edges

        /*
        float target = Random.insideUnitSphere + transform.position;
        target.y = transform.position.y; // Keep target on the same horizontal plane
        */

        
    }

    void Update()
    {
        // Get the steering direction from the navigation raycasts
        Vector3 navigate = Navigate();

        // Keep the enemy grounded with a small constant downward force
        // If airborne, accumulate gravity over time so it accelerates downward naturally
        if (character.isGrounded)
        {
            verticalVelocity = -0.2f;
        } 
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        
        // Build the final movement vector from vertical and forward components
        Vector3 upwardVelocity = transform.up * verticalVelocity;
        Vector3 forwardVelocity = transform.forward * moveSpeed * Time.deltaTime;
        Vector3 movement = upwardVelocity + forwardVelocity;

        

        // Only rotate if the navigate vector is significant enough
        // This prevents jittery spinning in open space where no walls are detected
        if(navigate.magnitude > 0.1f)
        {
            // navigate.y is the result of the cross product, positive means turn right, negative means turn left
            // Its magnitude determines how sharply to turn based on how close the nearest wall is
            transform.Rotate(0f, navigate.y * rotateSpeed * Time.deltaTime, 0f);
        }

        character.Move(movement);


        /*
        if(transform.position == target)
        {
            target = Random.insideUnitSphere + transform.position;
        }
        */

        Debug.Log(navigate);
    }

    Vector3 Navigate()
    {
        float centerIndex = numRays / 2f;

        rayDegree = rotationArc / (numRays - 1);

        forwardDegree = forwardArc / (forwardRays - 1);

        Vector3 rayAngle = transform.forward;
        Vector3 forwardAngle = transform.forward;
        Vector3 navigate = Vector3.zero;

        // Start at the left edge of the arc (-75 degrees from forward)
        rayAngle = Quaternion.AngleAxis(-(rotationArc / 2), Vector3.up) * rayAngle;

        forwardAngle = Quaternion.AngleAxis(-(forwardArc / 2), Vector3.up) * forwardAngle;

        int clearRays = 0;
        for(int i = 0; i < forwardRays; i++)
        {
            if(!Physics.Raycast(character.transform.position, forwardAngle, forwardDistance))
            {
                Debug.DrawRay(transform.position, forwardAngle * forwardDistance, Color.green);
                clearRays++;
            }
            
            forwardAngle = Quaternion.AngleAxis(forwardDegree, Vector3.up) * forwardAngle;
        }
        if(clearRays == forwardRays)
            {
                return Vector3.zero;
            }

        

        for(int i = 0; i < numRays; i++)
        {
            // Center rays have higher weight than outer rays
            // This makes the enemy more committed to moving forward through gaps like doorways
            float weight =  Mathf.Pow(centerIndex - Mathf.Abs(centerIndex - i), 2);

            RaycastHit hit;
            if(Physics.Raycast(character.transform.position, rayAngle, out hit, rayDistance))
            {
                Debug.DrawRay(transform.position, rayAngle * rayDistance, Color.red);

                // Push navigate away from the wall
                // Closer walls have stronger influence due to 1/hit.distance
                navigate -= rayAngle * (1 / hit.distance) * weight;
            }
            else
            {
                // No obstacle detected in this direction, draw green ray for debugging
                Debug.DrawRay(transform.position, rayAngle * rayDistance, Color.green);
            }

            // Rotate to the next ray angle in the arc
            rayAngle = Quaternion.AngleAxis(rayDegree, Vector3.up) * rayAngle;
        }

        // Cross product of forward and navigate gives a vector whose Y component
        // tells us whether to rotate left or right, and how much
        return Vector3.Cross(transform.forward, navigate);
    }
}