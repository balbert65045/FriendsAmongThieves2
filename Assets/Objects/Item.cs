using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    GameObject ObjectHeld;
    Player playerActive;

    public void SetPlayerChestandPlayer(Player player, GameObject Object)
    {
        playerActive = player;
        ObjectHeld = Object;
    }

    public void Clicked()
    {
        playerActive.TakeItemFromChest(ObjectHeld.GetComponent<usableItem>());
        Destroy(gameObject);
    }
}
