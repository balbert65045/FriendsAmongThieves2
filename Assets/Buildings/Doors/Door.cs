using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Door : MonoBehaviour {

    public enum DoorType { Red, Blue, Green, Gold, General }
    public DoorType ThisDoorType = DoorType.General;

    Animator d_Animator;

    public bool Open = false;

    public bool Closed = true;

	 void Start () {
        d_Animator = GetComponentInParent<Animator>();
    }


    public bool OpenCloseDoor(DoorType key)
    {

            if (ThisDoorType == DoorType.General)
            {
                Debug.Log("Opening Door");
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
                return true;
            }
            else if (key == ThisDoorType)
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
                return true;
            }
        return false;
    }
	
}
