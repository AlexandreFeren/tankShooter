using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Refill")]
public class AIRefillAction : AIAction
{
    public override void Act(AIStateController controller)
    {
        SeekRefill(controller);
    }

    private void SeekRefill(AIStateController controller)
    {
        if (controller.GetComponent<TankShooting>().m_Ammo < 1)
        {
            Vector3 overshoot = Vector3.Normalize(controller.navMeshAgent.transform.position - AmmoRefill.refillWaypoint);
            controller.navMeshAgent.destination = AmmoRefill.refillWaypoint + overshoot * controller.navMeshAgent.stoppingDistance;
            controller.navMeshAgent.isStopped = false;
        }
        else if (controller.GetComponent<TankHealth>().m_CurrentHealth < 30)
        {
            Vector3 overshoot = Vector3.Normalize(controller.navMeshAgent.transform.position - HPRefill.refillWaypoint);
            controller.navMeshAgent.destination = HPRefill.refillWaypoint + overshoot*controller.navMeshAgent.stoppingDistance;
            controller.navMeshAgent.isStopped = false;
        }

        controller.navMeshAgent.isStopped = false;

        /*
        if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance
            && !controller.navMeshAgent.pathPending)
        {
            if (controller.GetComponent<TankHealth>().m_CurrentHealth < 30)
            {
                controller.navMeshAgent.destination = HPRefill.refillWaypoint;
            }
            else if (controller.GetComponent<TankShooting>().m_Ammo < 1)
            {
                controller.navMeshAgent.destination = AmmoRefill.refillWaypoint;
            }
        }
        */
    }
}
