using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaSlider : MonoBehaviour {

    // Use this for initialization
    Slider slider;
    Player player; 
	void Start () {
        slider = GetComponent<Slider>();
    }

    public void SetPlayer(Player MyPlayer)
    {
        player = MyPlayer;
    }
	
	// Update is called once per frame
	void Update () {
        if (player != null)
        {
            slider.value = player.CurrentStamina;
        }
    }
}
