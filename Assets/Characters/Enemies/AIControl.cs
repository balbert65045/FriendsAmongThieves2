using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(EnemyCharacterControl))]
[RequireComponent(typeof(Enemy))]
public class AIControl : MonoBehaviour
{

    public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
    public EnemyCharacterControl character { get; private set; } // the character we are controlling
    private Enemy enemy;

    private enum EnemyStates { Patrol, Chase }
    private EnemyStates CurrentState = EnemyStates.Patrol;

    //private EnemyCharacterControl character;   //public 
    public Transform target;                                    // target to aim for


    Vector3 MoveSpeed;


    private void Start()
    {
        // get the components on the object we need ( should not be null due to require component so no need to check )
        agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        character = GetComponent<EnemyCharacterControl>();
        enemy = GetComponent<Enemy>();
     //   enemy.StatusChangeObservers += UpdateState;


        agent.updateRotation = false;
        agent.updatePosition = true;
    }


    private void Update()
    {
    
        if (target != null)
        {
            agent.SetDestination(target.position);
            if (CurrentState == EnemyStates.Patrol)
            {
                MoveSpeed = agent.desiredVelocity * .5f;
            }
            else if (CurrentState == EnemyStates.Chase)
            {
                MoveSpeed = agent.desiredVelocity * 1f;
            }
            else
            {
                Debug.LogError("UnknownState!!");
            }
        }

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            character.Move(MoveSpeed, false, false);
        }
        else
            character.Move(Vector3.zero, false, false);
    }

    public void UpdateState(Enemy.EnemyStates State)
    {
        switch (State)
        {
            case Enemy.EnemyStates.Patrol:
                CurrentState = EnemyStates.Patrol;
                break;
            case Enemy.EnemyStates.Chase:
                CurrentState = EnemyStates.Chase;
                break;
            default:
                return;
        }
    }


    public void SetTarget(Transform target)
    {
        this.target = target;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, target.transform.position);
    }
}
