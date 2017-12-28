using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    // Use this for initialization
    public int Rocks;
    public int SleepDarts;
    public int SmokeBombs;

    [SerializeField]
    GameObject BlueKeyHeld;
    [SerializeField]
    GameObject RedKeyHeld;
    [SerializeField]
    GameObject GreenKeyHeld;
    [SerializeField]
    GameObject GoldKeyHeld;

    public Key[] keys;

    void Start () {
        BlueKeyHeld.SetActive(false);
        RedKeyHeld.SetActive(false);
        GreenKeyHeld.SetActive(false);
        GoldKeyHeld.SetActive(false);
    }
	

    public void AddKey(Door.DoorType key)
    {
        switch (key)
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

}
