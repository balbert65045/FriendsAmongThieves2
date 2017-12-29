﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    GameObject ObjectHeld;
    Player playerActive;
    Chest chestActive;

    public void SetPlayerChestandPlayer(Player player, Chest chest, GameObject Object)
    {
        playerActive = player;
        chestActive = chest;
        ObjectHeld = Object;
    }

    public void Clicked()
    {
        playerActive.GetComponent<Inventory>().AddItem(ObjectHeld);
        chestActive.ItemsHeld.Remove(ObjectHeld);
        Destroy(this.gameObject);
    }
}