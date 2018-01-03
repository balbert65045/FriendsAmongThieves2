using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SoundSphere : MonoBehaviour {


    bool PlayerAttached;
    Transform Target; 

    public void SetTarget(Transform newTarget)
    {
        Target = newTarget;
        PlayerAttached = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (PlayerAttached)
        {
            transform.position = Target.position;
        }

    }
}
