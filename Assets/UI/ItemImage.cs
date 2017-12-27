using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemImage : MonoBehaviour {
    Player player;
    Image image;

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
        player = FindObjectOfType<Player>();
        player.notifyOnWinItemHelddObservers += WinItemHeld;
    }
	

    void WinItemHeld(bool ItemHeld)
    {
        if (ItemHeld)
        {
            float R = 1;
            float B = 1;
            float G = 1;
            float A = 1;
            image.color = new Color(R, B, G, A);
        }
        else
        {
            float R = 0;
            float B = 0;
            float G = 0;
            float A = 0;
            image.color = new Color(R, B, G, A);
        }
    }
	// Update is called once per frame
	void Update () {
		if (player.winObjectHeld)
        {

        }
	}
}
