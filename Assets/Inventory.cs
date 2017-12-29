using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    // Use this for initialization
    public List<rock> Rocks;
    public List<SleepDart> SleepDarts;
    public List<SmokeBomb> SmokeBombs; 



    public List<Key> keys;

    [SerializeField]
    GameObject BlueKeyHeld;
    [SerializeField]
    GameObject RedKeyHeld;
    [SerializeField]
    GameObject GreenKeyHeld;
    [SerializeField]
    GameObject GoldKeyHeld;

   

    void Start () {
        BlueKeyHeld.SetActive(false);
        RedKeyHeld.SetActive(false);
        GreenKeyHeld.SetActive(false);
        GoldKeyHeld.SetActive(false);
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
    }

    public void LoseItem(UsableObject usableObject)
    {
        switch (usableObject)
        {
            case UsableObject.Rock:
                Rocks.Remove(Rocks[Rocks.Count - 1]);
                break;
            case UsableObject.SleepDart:

                break;
            case UsableObject.SmokeBomb:
              
                break;
        }
    }

}
