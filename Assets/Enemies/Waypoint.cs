using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {
    
    // Use this for initialization
    WaypointSystem waypointSystem;
	void Start () {
        waypointSystem = GetComponentInParent<WaypointSystem>();
	}

 
    // Update is called once per frame
    void Update () {
		
	}
}
