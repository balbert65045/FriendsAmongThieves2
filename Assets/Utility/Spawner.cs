using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour {

    // Use this for initialization
    public Transform EnemyLocation;

    public override void OnStartServer()
    {
        Debug.Log("ServerStart");
        myNetworkManager MynetworkManager = FindObjectOfType<myNetworkManager>();
      //  PrefabLocations PrefabLocations = FindObjectOfType<PrefabLocations>();

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
            if (obj.GetComponent<Enemy>())
            {
                GameObject enemy = Instantiate(obj, EnemyLocation.position, EnemyLocation.rotation);
                NetworkServer.Spawn(enemy);
            }


        }

       
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
