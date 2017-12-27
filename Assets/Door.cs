using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    // Use this for initialization
    Animator d_Animator;
	void Start () {
        d_Animator = GetComponent<Animator>();
    }

    public void OpenCloseDoor()
    {
        d_Animator.SetTrigger("Action");
    }
	
}
