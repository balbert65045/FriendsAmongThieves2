using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Door : MonoBehaviour {

    public enum DoorType { Red, Blue, Green, Gold, General }
    public DoorType ThisDoorType = DoorType.General;

    Animator d_Animator;
    NetworkAnimator networkAnimator;

	void Start () {
        d_Animator = GetComponent<Animator>();
        networkAnimator = GetComponent<NetworkAnimator>();
    }

    public bool OpenCloseDoor(DoorType key)
    {
        if (ThisDoorType == DoorType.General)
        {
            Debug.Log("Opening Door");
            networkAnimator.SetTrigger("Action");
            return true;
        }
        else if (key == ThisDoorType)
        {
            networkAnimator.SetTrigger("Action");
            return true;
        }
        return false;
    }
	
}
