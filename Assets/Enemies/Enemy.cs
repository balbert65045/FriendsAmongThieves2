using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour {

    // Use this for initialization

    [SerializeField]
    float WalkSpeed = .5f;
    [SerializeField]
    float RunSpeed = 1f;
    [SerializeField]
    bool StationaryGuard = false;

    public enum EnemyStates { Patrol, Chase, InvestigateNoise, InvestigateSeight, Sleeping }
    public EnemyStates CurrentState = EnemyStates.Patrol;
    public VisionController visionController;
    public WaypointSystem waypointSystem;
    public float RemainingDistance;
    //public float RemainingDistance { get; private set;}

    AICharacterControl aICharacterControl;
    Vector3 PositionMovingTo;

    Vector3 BeginingLookArea;
    GameObject LookPoint;
    
   


    public void StatusChange(EnemyStates enemyState, Transform Location, Player playerChasing)
    {
      
        CurrentState = enemyState;
        if (CurrentState == EnemyStates.Sleeping)
        {
            aICharacterControl.agent.speed = 0;
            aICharacterControl.agent.isStopped = true;
        }
        else if (CurrentState == EnemyStates.Chase)
        {
            aICharacterControl.agent.speed = RunSpeed;
            aICharacterControl.SetTarget(playerChasing.transform);
        }
        else if (CurrentState == EnemyStates.Patrol)
        {
            aICharacterControl.agent.isStopped = false;
            aICharacterControl.agent.speed = WalkSpeed;
            aICharacterControl.SetTarget(waypointSystem.ActiveWaypoint);
        }
        else if (CurrentState == EnemyStates.InvestigateNoise)
        {
            aICharacterControl.SetTarget(Location);
            PositionMovingTo = Location.position;

        }
        else if (CurrentState == EnemyStates.InvestigateSeight)
        {

            aICharacterControl.SetTarget(Location);
            PositionMovingTo = Location.position;
        }
        //  if (StatusChangeObservers != null) StatusChangeObservers(enemyState);
    }


    void Start () {
        //waypointSystem = FindObjectOfType<WaypointSystem>();
   
        aICharacterControl = GetComponent<AICharacterControl>();
        aICharacterControl.SetTarget(waypointSystem.ActiveWaypoint);
        aICharacterControl.agent.speed = WalkSpeed;
        if (StationaryGuard)
        {
            BeginingLookArea = transform.position + transform.forward*2f;
            LookPoint = new GameObject("LookPoint");
            LookPoint.transform.SetParent(transform.parent.transform);
            LookPoint.transform.position = BeginingLookArea;
        }

    }

    private void Update()
    {

        if (CurrentState == EnemyStates.InvestigateSeight || CurrentState == EnemyStates.InvestigateNoise)
        {
            RemainingDistance = (PositionMovingTo - transform.position).magnitude;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CurrentState == EnemyStates.Patrol)
        {
            if (other.gameObject.GetComponent<Waypoint>() != null && other.transform == waypointSystem.ActiveWaypoint)
            {
                if (StationaryGuard)
                {
                    
                    aICharacterControl.SetTarget(LookPoint.transform);
                }
                else
                {
                    waypointSystem.UpdateWaypoint();
                    aICharacterControl.SetTarget(waypointSystem.ActiveWaypoint);

                }
            }   
        }
        
      
    }

}
