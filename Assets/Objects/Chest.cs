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
                Debug.Log(i);
                if (!itemLUT.Items.Contains(item)) { Debug.LogError("Item in chest is not in lookup table"); }
                else if (item == itemLUT.Items[i])
                {
                    ItemsIndexHeld.Add(i);
                    Debug.Log("Index Added");
                }
            }
        }
    }

    public override void OnStartClient()
    {
        ItemsIndexHeld.Callback = OnItemsUpdated;
    }

    private void OnItemsUpdated(SyncListInt.Operation op, int itemIndex)
    {
        Debug.Log("Items Changed");
    }


    public void OpenChest(Player playerOpening)
    {
        //  Debug.Log(ItemsIndexHeld);

        InUse = true;
        ChestStorageUI ChestUI = FindObjectOfType<ChestStorageUI>();
        ItemLookUpTable itemLUT = FindObjectOfType<ItemLookUpTable>();
        itemsHeld = new List<GameObject>();
        foreach (int Index in ItemsIndexHeld)
        {
            itemsHeld.Add(itemLUT.Items[Index]);
        }

        ChestUI.OpenChest(itemsHeld, playerOpening, this);
    }

    

    public void CloseChest()
    {
        InUse = false;
    }

   [ClientRpc]
    public void RpcTakeItemOut(int ItemIndex)
    {
        ItemLookUpTable itemLUT = FindObjectOfType<ItemLookUpTable>();
        ItemsIndexHeld.Remove(ItemIndex);
    }
}
