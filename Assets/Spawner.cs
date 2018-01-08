using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour {

    // Use this for initialization
    public override void OnStartServer()
    {
        Debug.Log("ServerStart");
        myNetworkManager MynetworkManager = FindObjectOfType<myNetworkManager>();
        DoorLocations doorlocals = FindObjectOfType<DoorLocations>();
        GameObject door = Instantiate(MynetworkManager.spawnPrefabs[1], doorlocals.doorPositions[0].position, doorlocals.doorPositions[0].rotation);
        NetworkServer.Spawn(door);
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
