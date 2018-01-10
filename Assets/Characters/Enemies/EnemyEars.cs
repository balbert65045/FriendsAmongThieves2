using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEars : MonoBehaviour {


    [SerializeField]
    float InvestigateDistance = 2f;
    public Vector3 CurrentHeardPosition;
    [SerializeField]
    float DistanceTolerance = .5f;
    // Use this for initialization
    float CurrentHeight;

    Enemy enemy;
    GameObject PlayerHeardLocation;
	void Start () {
        enemy = GetComponent<Enemy>();
        CurrentHeight = transform.position.y;
    }


    private void LateUpdate()
    {
        //TO DO Check if this works
        if (enemy.CurrentState == Enemy.EnemyStates.InvestigateNoise)
        {
            //Debug.Log(enemy.RemainingDistance);
            if (enemy.RemainingDistance <= DistanceTolerance)
            {
                enemy.StatusChange(Enemy.EnemyStates.Patrol, null, null);
            }
        }
    }

    // May have problems using Enemy capsule collider 
    // Possibly use own capsule collider for listening
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SoundSphere>())
        {
            //Debug.Log("Hit by Player Sound Sphere");
            if (enemy.CurrentState == Enemy.EnemyStates.Patrol)
            {
                Vector3 DirectionHeard = (other.transform.position - transform.position).normalized;
                Vector3 LocationtoMove = (DirectionHeard * InvestigateDistance) + transform.position;
                if (PlayerHeardLocation != null) { Destroy(PlayerHeardLocation); }
                PlayerHeardLocation = new GameObject("HeardLocation");
                PlayerHeardLocation.transform.SetParent(transform.parent.transform);
                PlayerHeardLocation.transform.position = new Vector3(LocationtoMove.x, CurrentHeight, LocationtoMove.z);
                CurrentHeardPosition = PlayerHeardLocation.transform.position;

                enemy.StatusChange(Enemy.EnemyStates.InvestigateNoise, PlayerHeardLocation.transform, null);
                //Debug.Log("Should Change Status");
            }
            else if (enemy.CurrentState == Enemy.EnemyStates.InvestigateNoise || enemy.CurrentState == Enemy.EnemyStates.InvestigateSeight)
            {
                Vector3 DirectionHeard = (other.transform.position - transform.position).normalized;
                Vector3 LocationtoMove = (DirectionHeard * InvestigateDistance) + transform.position;
                if (PlayerHeardLocation != null) { Destroy(PlayerHeardLocation); }
                PlayerHeardLocation = new GameObject("HeardLocation");
                PlayerHeardLocation.transform.SetParent(transform.parent.transform);
                PlayerHeardLocation.transform.position = new Vector3(LocationtoMove.x, CurrentHeight, LocationtoMove.z);
                CurrentHeardPosition = PlayerHeardLocation.transform.position;

                enemy.StatusChange(Enemy.EnemyStates.InvestigateNoise, PlayerHeardLocation.transform, null);
            }
        }
    }


    private void OnDrawGizmos()
    {
        if (PlayerHeardLocation != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(PlayerHeardLocation.transform.position, .4f);
        }
   }

}
