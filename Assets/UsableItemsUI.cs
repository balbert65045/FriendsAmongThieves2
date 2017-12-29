using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsableItemsUI : MonoBehaviour {

    public Sprite RockSprite;
    public Sprite DartSprite;
    public Sprite SmokeBombSprite;

    Sprite[] sprites;
    int spriteIndex = 0;

    Image currentImage;

	// Use this for initialization
	void Start () {
        sprites = new Sprite[] { RockSprite, DartSprite, SmokeBombSprite };
        currentImage = GetComponent<Image>();
        currentImage.sprite = sprites[spriteIndex];
    }
	
    public void SwitchObject()
    {
        spriteIndex = (spriteIndex == sprites.Length -1) ? 0 : spriteIndex + 1;
        currentImage.sprite = sprites[spriteIndex];
    }

	// Update is called once per frame
	void Update () {
		
	}
}
