using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestStorageUI : MonoBehaviour {

    // Use this for initialization
    public GameObject ChestUI;

    public Image key;
    public Image rock;
    public Image SleepDart;
    public Image SmokeBomb;

    public Transform[] Slots;

    Player player;

	void Start () {
        ChestUI.SetActive(false);
        player = FindObjectOfType<Player>();
    }
	
	// Update is called once per frame
	void Update () {
		if (ChestUI.activeSelf)
        {
            if (Input.GetButtonDown("Cancel") || player.GetComponent<Rigidbody>().velocity.magnitude > 1f)
            {

                ChestUI.SetActive(false);
                player.RelockCursor();
                foreach (Transform Slot in Slots)
                {
                    if (Slot.GetComponentInChildren<Item>() != null)
                    {
                        Destroy(Slot.GetComponentInChildren<Item>().gameObject);
                    }
                }
            }
        }
	}

    public void OpenChest(List<GameObject> items, Player player, Chest chest)
    {
        ChestUI.SetActive(true);
        int slotIndex = 0;
        foreach (GameObject item in items)
        {
            Image image = null;
            if (item.GetComponent<Key>() != null)
            {
                image = Instantiate(key, Slots[slotIndex]);
                switch (item.GetComponent<Key>().KeyType)
                {
                    case Door.DoorType.Blue:
                        image.color = Color.blue;
                        break;
                    case Door.DoorType.Red:
                        image.color = Color.red;
                        break;
                    case Door.DoorType.Green:
                        image.color = Color.green;
                        break;
                    case Door.DoorType.Gold:
                        image.color = Color.yellow;
                        break;
                }
                
            }
            else if (item.GetComponent<rock>())
            {
                image = Instantiate(rock, Slots[slotIndex]);
                image.color = Color.gray;
            }
            else if (item.GetComponent<SleepDart>())
            {
                image = Instantiate(SleepDart, Slots[slotIndex]);
            }
            else if (item.GetComponent<SmokeBomb>())
            {
                image = Instantiate(SmokeBomb, Slots[slotIndex]);
            }


            if (image == null) { Debug.LogError("item has no component linked to it"); }
            image.gameObject.GetComponent<Item>().SetPlayerChestandPlayer(player, chest, item);
            slotIndex++;
        }
    }

}
