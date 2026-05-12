using UnityEngine;

public class StalkerSpawnState : IStalkerState
{
    public void EnterState(StalkerAI stalker)
    {
        stalker.darknessTimer = 0f;
        stalker.IsVisible(false);
    }

    public void UpdateState(StalkerAI stalker)
    {
        if(stalker.cooldownTimer > 0f)
        {
            stalker.cooldownTimer -= Time.deltaTime;
            return;
        }

        if (!stalker.NearbyLight())
        {
            stalker.darknessTimer += Time.deltaTime;
        }
        else
        {
            stalker.darknessTimer = 0f;
        }

        if(stalker.darknessTreshold <= stalker.darknessTimer)
        {
            float roll = Random.Range(0f, 1f);
            if(roll <= stalker.spawnChance)
            {
                Vector3 spawnPoint = stalker.player.transform.position + Random.onUnitSphere * stalker.spawnRadius;
                spawnPoint.y = Mathf.Max(spawnPoint.y, stalker.player.transform.position.y);
                stalker.transform.position = spawnPoint;
                stalker.SwitchState(new StalkState());
            }
            else
            {
                stalker.darknessTimer = 0f;
            }
        }
    }

    public void ExitState(StalkerAI stalker)
    {
        stalker.cooldownTimer = stalker.cooldownDuration;
        stalker.IsVisible(true);
    }
}