using UnityEngine;

public class WanderState : IEnemyState
{
    public void EnterState(EnemyAI enemy)
    {
        
    }

    public void UpdateState(EnemyAI enemy)
    {
        Vector3 navigate = enemy.Navigate();

        // Only rotate if the navigate vector is significant enough
        // This prevents jittery spinning in open space where no walls are detected
        if(navigate.magnitude > 0.1f)
        {
            // navigate.y is the result of the cross product, positive means turn right, negative means turn left
            // Its magnitude determines how sharply to turn based on how close the nearest wall is
            enemy.transform.Rotate(0f, navigate.y *enemy.rotateSpeed * Time.deltaTime, 0f);
        }

        if (enemy.DetectPlayer())
        {
            enemy.SwitchState(new ChaseState());
        }

        Debug.Log(navigate);

    }

    public void ExitState(EnemyAI enemy)
    {
        
    }
}