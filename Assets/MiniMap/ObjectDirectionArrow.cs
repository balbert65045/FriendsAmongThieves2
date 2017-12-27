using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDirectionArrow : MonoBehaviour {


    [SerializeField]
    float ArrowDistance = 2f;
    [SerializeField]
    float CloseDistance = 5f;
    // Use this for initialization

    Vector3 GoalObjLocation;
    ItemObjective itemObjective;
    WinArea winArea;
    Player player;
    MeshRenderer meshRenderer;
    float OldZ;

	void Start () {
        itemObjective = FindObjectOfType<ItemObjective>();
        winArea = FindObjectOfType<WinArea>();
        GoalObjLocation = itemObjective.transform.position;
        player = FindObjectOfType<Player>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 playerPos = player.transform.position;

        // If the Win Object is held point towards Win zone
        if (player.winObjectHeld)
        {
             if (!meshRenderer.enabled)
            {
                meshRenderer.enabled = true;
                winArea.ActiveWinMiniMap();
            }
            Vector3 DirectionVector = new Vector3(winArea.transform.position.x - playerPos.x, 0, winArea.transform.position.z - playerPos.z).normalized;
            Debug.Log(DirectionVector);
            transform.localPosition = new Vector3(DirectionVector.x * ArrowDistance, 10, DirectionVector.z * ArrowDistance);

            transform.LookAt(winArea.transform.position);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

        }
        else
        {

            // Have the Arrow point towards the object to steal
            if (new Vector3(GoalObjLocation.x - playerPos.x, 0, GoalObjLocation.z - playerPos.z).magnitude < CloseDistance)
            {
                meshRenderer.enabled = false;
            }
            Vector3 DirectionVector = new Vector3(GoalObjLocation.x - playerPos.x, 0, GoalObjLocation.z - playerPos.z).normalized;
            Debug.Log(DirectionVector);
            transform.localPosition = new Vector3(DirectionVector.x * ArrowDistance, 10, DirectionVector.z * ArrowDistance);

            transform.LookAt(GoalObjLocation);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
        
    }
}
