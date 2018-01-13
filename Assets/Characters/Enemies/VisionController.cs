using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionController : MonoBehaviour {


    // Use this for initialization
    public Enemy enemy;


    private EnemySight[] EnemySights;
    private EnemySight PatrolVision;
    private EnemySight ChaseVision;

    Transform PlayerLocationLastSeen;
    public Transform PlayerLastSeen { get { return PlayerLocationLastSeen; } }

    Player PlayerChasing;
    public void SetPlayerChasing(Player newPlayerChasing){PlayerChasing = newPlayerChasing;}

    public void UpdatePlayerLocationLastSeen(Transform location)
    {
        PlayerLocationLastSeen = location;
    }

    public void StateChange(Enemy.EnemyStates State)
    {
        switch (State)
        {
            case Enemy.EnemyStates.Patrol:
                ChaseVision.gameObject.SetActive(false);
                PatrolVision.gameObject.SetActive(true);
                break;
            case Enemy.EnemyStates.Chase:
                if (PlayerChasing == null) { Debug.LogError("No player to chase pass through"); }
                ChaseVision.gameObject.SetActive(true);
                PatrolVision.gameObject.SetActive(false);
                break;
            case Enemy.EnemyStates.InvestigateNoise:

                break;
            case Enemy.EnemyStates.InvestigateSeight:

                break;
            default:
                Debug.LogWarning("Unknown State recieved by vision controller");
                return;
        }
        enemy.StatusChange(State, PlayerLocationLastSeen, PlayerChasing);
    }

    void Start()
    {
        EnemySights = GetComponentsInChildren<EnemySight>();
        foreach (EnemySight ES in EnemySights)
        {
            if (ES.VisionState == EnemySight.VisionType.Patrol)
            {
                PatrolVision = ES;
            }
            else if (ES.VisionState == EnemySight.VisionType.Chase)
            {
                ChaseVision = ES;
            }
            else
            {
                Debug.LogWarning("Unknown type of vision found on Enenmy");
            }
        }
        ChaseVision.gameObject.SetActive(false);
        PatrolVision.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {


        transform.position = enemy.transform.position;
        transform.rotation = enemy.transform.rotation;
    }

}

