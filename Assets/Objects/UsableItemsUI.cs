using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsableItemsUI : MonoBehaviour {

    public Sprite RockSprite;
    public Sprite DartSprite;
    public Sprite SmokeBombSprite;


    Image currentImage;
    Text QuantityText;

	// Use this for initialization
	void Start () {
        currentImage = GetComponentInChildren<Image>();
        QuantityText = GetComponentInChildren<Text>();
        currentImage.sprite = RockSprite;
    }
	
    public void SwitchObject(UsableObject useableObject, int amount)
    {
        switch (useableObject)
        {
            case UsableObject.Rock:
                currentImage.sprite = RockSprite;
                break;
            case UsableObject.SleepDart:
                currentImage.sprite = DartSprite;
                break;
            case UsableObject.SmokeBomb:
                currentImage.sprite = SmokeBombSprite;
            break;
        }
        QuantityText.text = "x " + amount.ToString();
    }

    public void ShowNewQuantity(int amount)
    {
        QuantityText.text = "x " + amount.ToString();
    }

	// Update is called once per frame
	void Update () {
		
	}
}
