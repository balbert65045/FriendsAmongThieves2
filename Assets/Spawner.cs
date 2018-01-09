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
        PrefabLocations PrefabLocations = FindObjectOfType<PrefabLocations>();

        int doorIndex = 0;
        int chestIndex = 0;
        foreach (GameObject obj in MynetworkManager.spawnPrefabs)
        {
            //if (obj.GetComponent<Door>())
            //{
            //    GameObject door = Instantiate(obj, PrefabLocations.doorPositions[doorIndex].position, PrefabLocations.doorPositions[doorIndex].rotation);
            //    NetworkServer.Spawn(door);
            //    doorIndex++;
            //}
            // if (obj.GetComponent<Chest>())
            //{
            //    GameObject chest = Instantiate(obj, PrefabLocations.chestPositions[chestIndex].position, PrefabLocations.chestPositions[chestIndex].rotation);
            //    NetworkServer.Spawn(chest);
            //   // chest.GetComponent<Chest>().SetUpChest();
            //    chestIndex++;
            //}
        }

       
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
