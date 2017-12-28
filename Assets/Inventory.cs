using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    // Use this for initialization
    public List<rock> Rocks;
    //public int SleepDarts;
    //public int SmokeBombs;

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

}
