using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaSlider : MonoBehaviour {

    // Use this for initialization
    Slider slider;
    Player player; 
	void Start () {
        player = FindObjectOfType<Player>();
        slider = GetComponent<Slider>();

    }
	
	// Update is called once per frame
	void Update () {

        slider.value = player.CurrentStamina;
    }
}
