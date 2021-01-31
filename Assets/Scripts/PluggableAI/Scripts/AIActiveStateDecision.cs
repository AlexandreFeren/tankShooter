using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/ActiveState")]
public class AIActiveStateDecision : AIDecision
{
    public override bool Decide(AIStateController controller)
    {
        bool chaseTargetIsActive = controller.chaseTarget.gameObject.activeInHierarchy;
        return chaseTargetIsActive;
    }
}
