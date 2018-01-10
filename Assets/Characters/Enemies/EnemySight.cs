using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour {


    [SerializeField]
    float DistanceTolerance = 1f;

    // Use this for initialization
    public enum VisionType { Patrol, Chase }
    public VisionType VisionState;
    public LayerMask HideableMask = -1;
    public Enemy enemy;

    GameObject LastSeenPlayerLocation;
    private VisionController visionController;
    private Enemy.EnemyStates enemyState;
    private Vector3 EnemyViewPoint;
    private float enemyheight;
    Player player;
    private MeshCollider meshCollider;


    void Start () {
        visionController = GetComponentInParent<VisionController>();
        enemyheight = enemy.gameObject.GetComponent<CapsuleCollider>().height;
        EnemyViewPoint = new Vector3(enemy.transform.position.x, GetComponentInParent<VisionController>().transform.position.y + enemyheight, enemy.transform.position.z);
    }
	
	// Update is called once per frame
	void LateUpdate() {
        enemyState = enemy.CurrentState;
        //TO DO investigate area before returning to patrol
        //TO DO add additional state Return where enemy runs back to patrol position instead of walking
        if (enemyState == Enemy.EnemyStates.InvestigateSeight)
        {
           if (enemy.RemainingDistance <= DistanceTolerance)
            {
                visionController.StateChange(Enemy.EnemyStates.Patrol);
            }
        }
    }

    private void CheckVisionForPlayer()
    {
        enemyState = enemy.CurrentState;
        EnemyViewPoint = new Vector3(enemy.transform.position.x, GetComponentInParent<VisionController>().transform.position.y + enemyheight, enemy.transform.position.z);


        float playerheight = player.GetComponent<CapsuleCollider>().height;
        Vector3 PlayerFeet = player.transform.position;
        Vector3 PlayerHead = new Vector3(player.transform.position.x, player.transform.position.y + playerheight, player.transform.position.z);

        Vector3 LookDirection1 = PlayerFeet - EnemyViewPoint;
        Vector3 LookDirection2 = PlayerHead - EnemyViewPoint;

        RaycastHit hit1; // Ray1
        RaycastHit hit2; // Ray2

        Debug.DrawLine(EnemyViewPoint, PlayerFeet);
        Debug.DrawLine(EnemyViewPoint, PlayerHead);

        bool hastHit = Physics.Raycast(EnemyViewPoint, LookDirection1, out hit1, LookDirection1.magnitude - .5f, HideableMask);
        bool hasHit2 = Physics.Raycast(EnemyViewPoint, LookDirection2, out hit2, LookDirection2.magnitude - .5f, HideableMask);

        if (enemyState == Enemy.EnemyStates.Patrol)
        {
            if ((hastHit) && (hasHit2))
            {
                //Debug.Log(hit1.transform.gameObject.name);
                //Debug.Log(hit2.transform.gameObject.name);
                //Debug.Log("Player Hidden. Player Hidding behind " + hit1.transform.name + hit2.transform.name);
            }
            else
            {
                visionController.StateChange(Enemy.EnemyStates.Chase);
                visionController.SetPlayerChasing(player);
            }
        }

        else if (enemyState == Enemy.EnemyStates.Chase)
        {
            //Debug.Log("Chasing");
            if (LastSeenPlayerLocation != null) { Destroy(LastSeenPlayerLocation); }
            if ((hastHit) && (hasHit2))
            {
                CreateLastSeenLocation();

            }
            else
            {
                //Debug.Log("Player seen continue chasing");
            }
        }

        else if (enemyState == Enemy.EnemyStates.InvestigateSeight || enemyState == Enemy.EnemyStates.InvestigateNoise)
        {
            Debug.Log("Investigating");
            if ((hastHit) && (hasHit2))
            {
                //Debug.Log(hit1.transform.gameObject.name);
                //Debug.Log(hit2.transform.gameObject.name);
                //Debug.Log("Player Hidden. Player Hidding behind " + hit1.transform.name + hit2.transform.name);
            }
            else
            {
                visionController.StateChange(Enemy.EnemyStates.Chase);
                visionController.SetPlayerChasing(player);
            }
        }
      }


    void CreateLastSeenLocation()
    {
        if (LastSeenPlayerLocation != null) { Destroy(LastSeenPlayerLocation); }
        LastSeenPlayerLocation = new GameObject("PlayerLocation");
        LastSeenPlayerLocation.transform.SetParent(enemy.transform.parent);
        //CapsuleCollider Collider = LastSeenPlayerLocation.AddComponent<CapsuleCollider>();
        //Collider.isTrigger = true;
        LastSeenPlayerLocation.transform.position = player.transform.position;
        visionController.UpdatePlayerLocationLastSeen(LastSeenPlayerLocation.transform);
        visionController.StateChange(Enemy.EnemyStates.InvestigateSeight);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            //Debug.Log("PlayerHit");
            player = other.gameObject.GetComponent<Player>();
            CheckVisionForPlayer();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null && enemyState == Enemy.EnemyStates.Chase)
       {
            CreateLastSeenLocation();
        }

    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(EnemyViewPoint, .4f);
        if (LastSeenPlayerLocation != null) {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(LastSeenPlayerLocation.transform.position, .4f);
        }
        
    }

}
