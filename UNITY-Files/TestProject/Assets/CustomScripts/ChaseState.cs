using UnityEngine;

public class ChaseState : IEnemyState
{
    private float originalSpeed;

    private float lostTimer = 2f;
    private float lostTimerReset = 2f;


    public void EnterState(EnemyAI enemy)
    {
        originalSpeed = enemy.moveSpeed;
        enemy.moveSpeed = enemy.moveSpeed * 2;
    }


    public void UpdateState(EnemyAI enemy)
    {

        Vector3 navigate = enemy.Navigate();

        Vector3 playerDirection = enemy.player.transform.position - enemy.transform.position;
        playerDirection.y = 0;

        Vector3 direction = Vector3.Cross(enemy.transform.forward, playerDirection);


        if(navigate.magnitude > 1000f)
        {
            // navigate.y is the result of the cross product, positive means turn right, negative means turn left
            // Its magnitude determines how sharply to turn based on how close the nearest wall is
            enemy.transform.Rotate(0f, navigate.y *enemy.rotateSpeed * Time.deltaTime, 0f);
        }
        else
        {
            enemy.transform.Rotate(direction);
        }


        
        if(enemy.DetectPlayer())
        {
            lostTimer = lostTimerReset; // reset timer when player is spotted
        }
        else
        {
            lostTimer -= Time.deltaTime;
            if(lostTimer <= 0)
            {
                enemy.SwitchState(new WanderState());
            }
        }


        if(Vector3.Distance(enemy.transform.position, enemy.player.transform.position) < 1f)
        {
            enemy.SwitchState(new AttackState());
        }

        
    }

    public void ExitState(EnemyAI enemy)
    {
        enemy.moveSpeed = originalSpeed;
    }
}