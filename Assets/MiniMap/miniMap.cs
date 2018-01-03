using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniMap : MonoBehaviour {

    // Use this for initialization
    Player player;

    public void SetPlayer(Player MyPlayer)
    {
        player = MyPlayer;
    }

    // Update is called once per frame
    void Update () {
        if (player != null)
        {
            transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        }
	}
}
