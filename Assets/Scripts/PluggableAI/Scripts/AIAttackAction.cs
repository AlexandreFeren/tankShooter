using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Attack")]
public class AIAttackAction : AIAction
{
    public override void Act(AIStateController controller)
    {
        Attack(controller);
    }
    public void Attack(AIStateController controller)
    {
        //Debug.Log("Attack");
        RaycastHit hit;

        Debug.DrawRay(controller.eyes.position,
            controller.eyes.forward.normalized * controller.enemyStats.attackRange,
            Color.red);
        //Debug.Log(controller.enemyStats.attackRange);
        if (Physics.SphereCast(controller.eyes.position, controller.enemyStats.lookSphereCastRadius,
            controller.eyes.forward, out hit, controller.enemyStats.attackRange)
            && hit.collider.CompareTag("Player"))
        {
            //Debug.Log("Looking at Player");
            if (controller.CheckIfCountdownElapsed(controller.enemyStats.attackRate))
            {
                controller.tankShooting.Fire(Random.Range(0, controller.enemyStats.attackForce), controller.enemyStats.attackRate);
            }
        }
    }
}
