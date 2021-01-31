using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableAI/Decisions/Refill")]
public class AIRefillDecision : AIDecision
{
    TankShooting ShootScript;
    TankHealth HealthScript;
    // Start is called before the first frame update
    public override bool Decide(AIStateController controller)
    {
        ShootScript = controller.GetComponent<TankShooting>();
        HealthScript = controller.GetComponent<TankHealth>();
        if (ShootScript.m_Ammo < 1 || HealthScript.m_CurrentHealth < 30)
        {
            controller.navMeshAgent.stoppingDistance = 0;
            return true;
        }
        else
        {
            controller.navMeshAgent.stoppingDistance = 8;
            return false;

        }
    }
}
