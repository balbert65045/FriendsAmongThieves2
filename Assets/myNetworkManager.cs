﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class myNetworkManager : NetworkManager {

    public void MyStartHost()
    {
        StartHost();
        Debug.Log(Time.timeSinceLevelLoad + " Host started");
    }

    public override void OnStartHost()
    {
        base.OnStartHost();

        //door.transform.position = doorlocals.doorPositions[0].position;
        //door.transform.rotation = doorlocals.doorPositions[0].rotation;
        //DoorLocations doorlocals = FindObjectOfType<DoorLocations>();

        //GameObject door = Instantiate(spawnPrefabs[1], doorlocals.doorPositions[0].position, doorlocals.doorPositions[0].rotation);
        //Debug.Log(door.transform.position);
        //Debug.Log(doorlocals.doorPositions[0].position);

        Debug.Log(Time.timeSinceLevelLoad + " Host requested");
    }

    public override void OnStartClient(NetworkClient myClient)
    {
        base.OnStopClient();
        Debug.Log(Time.timeSinceLevelLoad + " Client start requested" );
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        Debug.Log(Time.timeSinceLevelLoad + " Client connected to IP:" + conn.address);
        Debug.Log(Network.player.ipAddress);

        DoorLocations doorlocals = FindObjectOfType<DoorLocations>();
        GameObject door = Instantiate(spawnPrefabs[1], doorlocals.doorPositions[0].position, doorlocals.doorPositions[0].rotation);
        NetworkServer.Spawn(door);

    }
}
