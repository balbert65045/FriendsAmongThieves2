﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestStorage : MonoBehaviour {

    // Use this for initialization
    public GameObject ChestUI;

    public Image key;
    public Image rock;

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
            if (Input.GetButtonDown("Cancel") || player.GetComponent<Rigidbody>().velocity.magnitude > .5f)
            {
                ChestUI.SetActive(false);
            }
        }
	}

    public void OpenChest(List<GameObject> items)
    {
        ChestUI.SetActive(true);
        int slotIndex = 0;
        foreach (GameObject item in items)
        {
            if (item.GetComponent<Key>() != null)
            {
                Image image = Instantiate(key, Slots[slotIndex]);
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
                Image image = Instantiate(rock, Slots[slotIndex]);
                image.color = Color.gray;
            }

            slotIndex++;
        }
    }

}
