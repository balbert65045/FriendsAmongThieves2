using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Chest : NetworkBehaviour {

    public List<GameObject> itemsHeld;
    public SyncList<int> ItemsIndexHeld;


    public void SetUpChest()
    {
        ItemLookUpTable itemLUT = FindObjectOfType<ItemLookUpTable>();
        foreach (GameObject item in itemsHeld)
        {
            for (int i = 0; i < itemLUT.Items.Count; i++)
            {
              //  Debug.Log(i);
                if (!itemLUT.Items.Contains(item)) { Debug.LogError("Item in chest is not in lookup table"); }
                else if (item == itemLUT.Items[i])
                {
                    ItemsIndexHeld.Add(i);
                    Debug.Log("Index Added");
                }
            }
        }
    }


    public void OpenChest(Player playerOpening)
    {
        Debug.Log(ItemsIndexHeld);
        ChestStorageUI ChestUI = FindObjectOfType<ChestStorageUI>();
        ItemLookUpTable itemLUT = FindObjectOfType<ItemLookUpTable>();
        itemsHeld = new List<GameObject>();
        foreach (int Index in ItemsIndexHeld)
        {
            itemsHeld.Add(itemLUT.Items[Index]);
        }
         
        ChestUI.OpenChest(itemsHeld, playerOpening, this);
    }

    public void TakeItemOut(GameObject item)
    {
        ItemLookUpTable itemLUT = FindObjectOfType<ItemLookUpTable>();
        for (int i = 0; i < itemLUT.Items.Count; i++)
        {
            if (item == itemLUT.Items[i])
            {
                ItemsIndexHeld.Remove(i);
            }
        }
    }
}
