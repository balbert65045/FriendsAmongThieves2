using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Chest : NetworkBehaviour {

    public List<GameObject> itemsHeld;
    public SyncListInt ItemsIndexHeld = new SyncListInt();
    [SyncVar]
    public bool InUse = false;

    public override void OnStartServer()
    {
        ItemLookUpTable itemLUT = FindObjectOfType<ItemLookUpTable>();
        foreach (GameObject item in itemsHeld)
        {
            for (int i = 0; i < itemLUT.Items.Count; i++)
            {
                if (!itemLUT.Items.Contains(item)) { Debug.LogError("Item in chest is not in lookup table"); }
                else if (item == itemLUT.Items[i])
                {
                    ItemsIndexHeld.Add(i);
                }
            }
        }
    }

    public override void OnStartClient()
    {
        ItemsIndexHeld.Callback = OnItemsUpdated;
    }

    // used if something changes with the list
    private void OnItemsUpdated(SyncListInt.Operation op, int itemIndex)
    {
    }


    public void OpenChest(Player playerOpening)
    {
        //  Debug.Log(ItemsIndexHeld);
        ChestStorageUI ChestUI = FindObjectOfType<ChestStorageUI>();
        ItemLookUpTable itemLUT = FindObjectOfType<ItemLookUpTable>();
        itemsHeld = new List<GameObject>();
        foreach (int Index in ItemsIndexHeld)
        {
            itemsHeld.Add(itemLUT.Items[Index]);
        }

        ChestUI.OpenChest(itemsHeld, playerOpening, this);
    }

    public void SetChestOpen()
    {
        InUse = true;
    }

    public void SetCloseChest()
    {
        InUse = false;
    }

    public void TakeItemOut(int ItemIndex)
    {
        ItemLookUpTable itemLUT = FindObjectOfType<ItemLookUpTable>();
        ItemsIndexHeld.Remove(ItemIndex);
    }
}
