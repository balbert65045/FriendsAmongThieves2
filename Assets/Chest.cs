using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    public List<GameObject> ItemsHeld;

	// Use this for initialization
	void Start () {
		
	}
	
	public void OpenChest(Player playerOpening)
    {
        ChestStorage ChestUI = FindObjectOfType<ChestStorage>();
        //Inventory playerInventory = playerOpening.GetComponent<Inventory>();
        //foreach (GameObject item in ItemsHeld)
        //{
        //    playerInventory.AddItem(item);
        //}
        ChestUI.OpenChest(ItemsHeld, playerOpening, this);
        //ItemsHeld.Clear();
    }
}
