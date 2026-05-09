using UnityEngine;
public class AttackState : IEnemyState
{
    public void EnterState(EnemyAI enemy)
    {
        enemy.player.TakeDamage(enemy.player.maxHealth);
    }

    public void UpdateState(EnemyAI enemy)
    {
        
    }

    public void ExitState(EnemyAI enemy)
    {
        enemy.SwitchState(new WanderState());
    }
}