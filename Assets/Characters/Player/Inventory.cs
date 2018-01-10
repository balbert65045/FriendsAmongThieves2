using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    // Use this for initialization
    public List<rock> Rocks;
    public List<SleepDart> SleepDarts;
    public List<SmokeBomb> SmokeBombs; 



    public List<Key> keys;


   public GameObject BlueKeyHeld;
   public GameObject RedKeyHeld;
   public GameObject GreenKeyHeld;
   public GameObject GoldKeyHeld;

   public void LinkKey(KeyImage key)
    {
        switch (key.KeyType)
        {
            case Door.DoorType.Blue:
                BlueKeyHeld = key.gameObject;
                BlueKeyHeld.SetActive(false);
                break;
            case Door.DoorType.Red:
                RedKeyHeld = key.gameObject;
                RedKeyHeld.SetActive(false);
                break;
            case Door.DoorType.Green:
                GreenKeyHeld = key.gameObject;
                GreenKeyHeld.SetActive(false);
                break;
            case Door.DoorType.Gold:
                GoldKeyHeld = key.gameObject;
                GoldKeyHeld.SetActive(false);
                break;
        }
    }

	
    public int QuantityCheck(UsableObject usableObject)
    {
        switch (usableObject)
        {
            case UsableObject.Rock:
                return Rocks.Count;
            case UsableObject.SleepDart:
                return SleepDarts.Count;
            case UsableObject.SmokeBomb:
                return SmokeBombs.Count;
        }
        return 0;
    }

    public void AddItem(GameObject Item)
    {
        if (Item.GetComponent<Key>())
        {
            keys.Add(Item.GetComponent<Key>());
            Door.DoorType keyType = Item.GetComponent<Key>().KeyType;
            switch (keyType)
            {
                case Door.DoorType.Blue:
                    BlueKeyHeld.SetActive(true);
                    break;
                case Door.DoorType.Red:
                    RedKeyHeld.SetActive(true);
                    break;
                case Door.DoorType.Green:
                    GreenKeyHeld.SetActive(true);
                    break;
                case Door.DoorType.Gold:
                    GoldKeyHeld.SetActive(true);
                    break;
            }
        }
        else if (Item.GetComponent<rock>())
        {
            Rocks.Add(Item.GetComponent<rock>());
        }
        else if (Item.GetComponent<SleepDart>())
        {
            SleepDarts.Add(Item.GetComponent<SleepDart>());
        }
        else if (Item.GetComponent<SmokeBomb>())
        {
            SmokeBombs.Add(Item.GetComponent<SmokeBomb>());
        }
    }

    public void LoseItem(UsableObject usableObject)
    {
        switch (usableObject)
        {
            case UsableObject.Rock:
                Rocks.Remove(Rocks[Rocks.Count - 1]);
                break;
            case UsableObject.SleepDart:
                SleepDarts.Remove(SleepDarts[SleepDarts.Count - 1]);
                break;
            case UsableObject.SmokeBomb:
                SmokeBombs.Remove(SmokeBombs[SmokeBombs.Count - 1]);
                break;
        }
    }

}
