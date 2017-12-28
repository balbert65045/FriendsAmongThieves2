using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public enum DoorType { Red, Blue, Green, Gold, General }
    public DoorType ThisDoorType = DoorType.General;

    // Use this for initialization
    Animator d_Animator;
	void Start () {
        d_Animator = GetComponent<Animator>();
    }

    public bool OpenCloseDoor(DoorType key)
    {
        if (ThisDoorType == DoorType.General)
        {
            Debug.Log("Opening Door");
            d_Animator.SetTrigger("Action");
            return true;
        }
        else if (key == ThisDoorType)
        {
            d_Animator.SetTrigger("Action");
            return true;
        }
        return false;
    }
	
}
