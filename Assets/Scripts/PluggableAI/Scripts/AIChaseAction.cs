using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
public class AIChaseAction : AIAction
{
    public override void Act(AIStateController controller)
    {
        Chase(controller);
    }

    private void Chase(AIStateController controller)
    {
        Vector3 side1 = (controller.chaseTarget.position - controller.transform.position).normalized;
        Vector3 side2 = (controller.eyes.forward);

        controller.navMeshAgent.destination = controller.chaseTarget.position;
        controller.navMeshAgent.isStopped = false;
    }
}