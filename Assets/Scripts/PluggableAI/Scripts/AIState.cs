﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenuAttribute(menuName = "PluggableAI/State")]
public class AIState : ScriptableObject
{

    public AIAction[] actions;
    public AITransition[] transitions;
    public Color SceneGizmoColor = Color.gray;


    public void UpdateState(AIStateController controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }
    private void DoActions(AIStateController controller)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            //Debug.Log("AIState UpdateState " + actions[i]);
            actions[i].Act(controller);
        }
    }
    private void CheckTransitions(AIStateController controller)
    {
        for (int i = 0; i < transitions.Length; i++)
        {

            bool decisionSucceeded = transitions[i].decision.Decide(controller);

            if (decisionSucceeded)
            {
                controller.TransitionToState(transitions[i].trueState);
            }
            else
            {
                controller.TransitionToState(transitions[i].falseState);
            }
        }
    }
}
