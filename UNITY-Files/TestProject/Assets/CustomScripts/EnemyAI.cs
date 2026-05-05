using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;

    public float rayDistance = 3f;

    public int numRays = 5;

    public float rotateSpeed = 45f;

    
    private float rayDegree;

    private float verticalVelocity;

    private CharacterController character;
    private Player player; // Will be used to spot the player later on




    void Start()
    {
        character = GetComponent<CharacterController>();
        rayDegree = 150f / (numRays - 1);
    }

    void Update()
    {

        Vector3 navigate = Navigate();

        if (character.isGrounded)
        {
            verticalVelocity = -0.5f;
        } 
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        
        Vector3 upwardVelocity = transform.up * verticalVelocity;

        Vector3 forwardVelocity = transform.forward * moveSpeed * Time.deltaTime;

        Vector3 movement = upwardVelocity + forwardVelocity;

        

        character.Move(movement);

        
        if(navigate.magnitude > 0.1f)
        {
            transform.Rotate(0f, navigate.y * rotateSpeed * Time.deltaTime, 0f);
        }
        
        

        Debug.Log(navigate);
    }


    Vector3 Navigate()
    {
        float centerIndex = numRays / 2f;

        Vector3 rayAngle = transform.forward;
        Vector3 navigate = new Vector3 (0, 0, 0);

        rayAngle = Quaternion.AngleAxis(-75, Vector3.up) * rayAngle;

        for(int i = 0; i < numRays; i++)
        {
            float weight = 2 * (centerIndex - Mathf.Abs(centerIndex - i));

            RaycastHit hit;
            if(Physics.Raycast(character.transform.position, rayAngle, out hit, rayDistance))
            {
                Debug.DrawRay(transform.position, rayAngle * rayDistance, Color.red);
                navigate -= rayAngle * (1 / hit.distance) * weight;
            }
            else
            {
                Debug.DrawRay(transform.position, rayAngle * rayDistance, Color.green);
            }
             

            rayAngle = Quaternion.AngleAxis(rayDegree, Vector3.up) * rayAngle;
        }
        return Vector3.Cross(transform.forward, navigate);
    }
}