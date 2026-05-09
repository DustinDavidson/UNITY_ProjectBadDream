using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Player player; // Used to let the AI know what to chase
    public float moveSpeed = 2f;

    public float forwardDistance = 5f;

    public float rayDistance = 3.5f;  

    // Number of rays to cast in the navigation arc
    public int numRays = 40;

    public int forwardRays = 10;

    public float rotateSpeed = 1f;

    public float rotationArc = 180f;

    public float forwardArc = 10f;

    [Header("Detection Settings")]

    public float detectRange = 20;

    public int wCount = 10;

    public int hCount = 7;

    public float detectWidth = 90f;

    public float detectHeight = 45f;

    private IEnemyState currentState;

    // Tracks vertical speed, accumulates over time when airborne to simulate gravity
    private float verticalVelocity;

    private CharacterController character;

    void Start()
    {
        character = GetComponent<CharacterController>();
        currentState = new WanderState();
        currentState.EnterState(this);
        
    }

    void Update()
    {
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

    
        currentState.UpdateState(this);
        character.Move(movement);
         
    }


    public void SwitchState(IEnemyState newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);   
    }


    public Vector3 Navigate()
    {
        // Angle between each ray, calculated from the total arc (150 degrees) divided by the number of gaps between rays
        float rayDegree;

        float forwardDegree;

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
            if(!Physics.Raycast(transform.position, forwardAngle, forwardDistance))
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
            if(Physics.Raycast(transform.position, rayAngle, out hit, rayDistance))
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
        Vector3 result = Vector3.Cross(transform.forward, navigate);
        return result;
    }


    public bool DetectPlayer()
    {
        Vector3 rayAngle = transform.forward;
        rayAngle = Quaternion.AngleAxis(-(detectWidth / 2), transform.up) * rayAngle;
        rayAngle = Quaternion.AngleAxis(-(detectHeight / 2), transform.right) * rayAngle;

        Vector3 rowStartAngle = rayAngle;

        float wDegree = detectWidth / (wCount - 1);
        float hDegree = detectHeight / (hCount - 1);



        for(int i = 0; i < hCount; i++)
        {
            for(int j = 0; j < wCount; j++)
            {
                RaycastHit hit;
                if(Physics.Raycast(character.transform.position, rayAngle, out hit, detectRange))
                {
                    Debug.DrawRay(transform.position, rayAngle * detectRange, Color.green);
                    if(hit.collider.tag == "Player")
                    {
                        Debug.DrawRay(transform.position, rayAngle * detectRange, Color.red);
                        return true;
                    }
                }
                else
                {
                    Debug.DrawRay(transform.position, rayAngle * detectRange, Color.green);
                }
                
                rayAngle = Quaternion.AngleAxis(wDegree, transform.up) * rayAngle;
            }
            rayAngle = rowStartAngle;
            rayAngle = Quaternion.AngleAxis(hDegree, transform.right) * rayAngle;
            rowStartAngle = rayAngle;
        }
        return false;    
    }
}