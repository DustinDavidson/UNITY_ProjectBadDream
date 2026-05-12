using UnityEngine;

public class StalkerAI : MonoBehaviour
{
    public Player player;
    public FlashLight flashlight;
    public float moveSpeed = 2f;

    [Header("Spawn Settings")]
    public float spawnRadius = 3f;
    public float darknessTimer = 0f;
    public float darknessTreshold = 3f;
    public float cooldownTimer = 0f;
    public float cooldownDuration = 5f;
    public float spawnChance = 0.25f;
    public float lightCheckRadius = 15f;

    private IStalkerState currentState;


    void Start()
    {
        currentState = new StalkerSpawnState();
        currentState.EnterState(this);
    }

    void Update()
    {

        currentState.UpdateState(this);
    }




    public bool NearbyLight()
    {
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, lightCheckRadius);
        foreach(Collider col in colliders)
        {
            if(col.gameObject.tag == "Light")
            {
                Vector3 directionToLight = col.transform.position - player.transform.position;
                RaycastHit hit;
                if(Physics.Raycast(player.transform.position, directionToLight, out hit, lightCheckRadius))
                {
                    if(hit.collider.gameObject.tag == "Light")
                    {
                        return true;
                    }
                }
            }
            
        }
         return false;       
    }

    public void SwitchState(IStalkerState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }
        currentState = newState;
        currentState.EnterState(this);
    }

    public void IsVisible(bool visible)
    {
        GetComponent<MeshRenderer>().enabled = visible;
    }
    
}