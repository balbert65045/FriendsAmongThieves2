using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSphere : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    bool PlayerAttached;
    Player player;

	void Start () {
        player = FindObjectOfType<Player>();

    }
	
	// Update is called once per frame
	void Update () {
        if (PlayerAttached)
        {
            transform.position = player.transform.position;
        }

    }
}
