//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using Complete;


public class AIStateController : MonoBehaviour
{

    public AIState currentState;
    public AIStats enemyStats;
    public Transform eyes;
    public AIState remainState;

    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public TankShooting tankShooting;
    [HideInInspector] public List<Transform> wayPointList;
    [HideInInspector] public int nextWaypoint;
    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public float stateTimeElapsed = 0;

    private bool aiActive;


    void Awake()
    {
        tankShooting = GetComponent<TankShooting>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void SetupAI(bool aiActivationFromTankManager, List<Transform> wayPointsFromTankManager)
    {
        wayPointList = wayPointsFromTankManager;
        aiActive = aiActivationFromTankManager;
        if (aiActive)
        {
            navMeshAgent.enabled = true;
        }
        else
        {
            navMeshAgent.enabled = false;
        }
    }

    private void Update()
    {
        //Debug.Log("Update");
        if (!aiActive)
        {
            return;
        }
        if (this == null) {
            Debug.Log("AIStateController Update");
        }
        
        //Debug.Log("AIStateController this: " + this);
        currentState.UpdateState(this);
    }
    private void OnDrawGizmos()
    {
        if (currentState != null && eyes != null)
        {
            Gizmos.color = currentState.SceneGizmoColor;
            Gizmos.DrawWireSphere(eyes.position, enemyStats.lookSphereCastRadius);
        }
    }
    public void TransitionToState(AIState nextState)
    {
        //Debug.Log("TransitionToState");
        //Debug.Log(remainState);
        //Debug.Log(nextState);
        if (nextState != remainState)
        {
            currentState = nextState;
        }
    }
    public bool CheckIfCountdownElapsed(float duration)
    {
        //timer for AI shots AKA don't be too unfair
        stateTimeElapsed += Time.deltaTime;
        return stateTimeElapsed >= duration;
    }
    private void OnExitState()
    {
        //use this to reset values on state transition
        stateTimeElapsed = 0;
    }
}