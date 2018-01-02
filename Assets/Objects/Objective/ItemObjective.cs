using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjective : MonoBehaviour {

    Player player;
    // Use this for initialization
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            player = other.GetComponent<Player>();
            if (player.PlayerAction)
            {
                Destroy(gameObject);
                player.ObjectGrabbed();
            }
        }
    }
}
