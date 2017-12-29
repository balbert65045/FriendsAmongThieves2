using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    public List<GameObject> ItemsHeld;
	
	public void OpenChest(Player playerOpening)
    {
        ChestStorageUI ChestUI = FindObjectOfType<ChestStorageUI>();
        ChestUI.OpenChest(ItemsHeld, playerOpening, this);
    }
}
