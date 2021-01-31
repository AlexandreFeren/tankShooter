using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Look")]
public class AILookDecision : AIDecision
{
    public override bool Decide(AIStateController controller)
    {

        bool targetVisible = Look(controller);
        return targetVisible;
    }
    private bool Look(AIStateController controller)
    {
        GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        RaycastHit hit;

        Debug.DrawRay(controller.eyes.position,
            controller.eyes.forward.normalized * controller.enemyStats.lookRange,
            Color.green);

        if (Physics.SphereCast(controller.eyes.position, controller.enemyStats.lookSphereCastRadius,
            controller.eyes.forward, out hit, controller.enemyStats.lookRange)
            && hit.collider.CompareTag("Player"))
        {
            controller.chaseTarget = hit.transform;
            //Debug.Log("Look True");
            return true;
        }
        else if (Vector3.Distance(controller.transform.position, manager.m_Tanks[0].m_Instance.transform.position) < 8)
        {
            //Debug.Log("Look False");
            controller.chaseTarget = manager.m_Tanks[0].m_Instance.transform;
            return true;
        }
        else
        {
            return false;
        }
    }
}
