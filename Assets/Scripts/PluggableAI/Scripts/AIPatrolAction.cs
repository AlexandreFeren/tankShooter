using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Patrol")]
public class AIPatrolAction : AIAction
{

    public override void Act(AIStateController controller)
    {
        Patrol(controller);
    }


    private void Patrol(AIStateController controller)
    {
        controller.navMeshAgent.destination = controller.wayPointList[controller.nextWaypoint].position;
        controller.navMeshAgent.isStopped = false;

        if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance
            && !controller.navMeshAgent.pathPending)
        {
            //circularly go through waypoint list, occasionally deviate

            if (Random.Range(0, 5) != 0)
            {
                controller.nextWaypoint = Random.Range(1, controller.wayPointList.Count);
            }
            else
            {
                controller.nextWaypoint = (controller.nextWaypoint + 1) % controller.wayPointList.Count;
            }
        }

    }
}
