using UnityEngine;

public interface IStalkerState
{
    void EnterState(StalkerAI stalker);
    void UpdateState(StalkerAI stalker);
    void ExitState(StalkerAI stalker);
    
}