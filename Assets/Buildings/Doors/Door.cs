using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Door : NetworkBehaviour {

    public enum DoorType { Red, Blue, Green, Gold, General }
    public DoorType ThisDoorType = DoorType.General;

    Animator d_Animator;
    [SyncVar]
    public bool Open = false;
    [SyncVar]
    public bool Closed = true;

	 void Start () {
        d_Animator = GetComponentInParent<Animator>();
    }



    [ClientRpc]
    public void RpcOpenCloseDoor()
    {
            if (Closed)
            {
                d_Animator.SetTrigger("Open");
                Closed = false;
                Open = true;
            }
            else if (Open)
            {
                d_Animator.SetTrigger("Closed");
                Closed = true;
                Open = false;
            }
    }
	
}
