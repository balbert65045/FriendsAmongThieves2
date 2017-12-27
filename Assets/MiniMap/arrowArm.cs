using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowArm : MonoBehaviour {

    // Use this for initialization
    Player player;
	void Start () {
        player = FindObjectOfType<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = player.transform.position;

    }
}
