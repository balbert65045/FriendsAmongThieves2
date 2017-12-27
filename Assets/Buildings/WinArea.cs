using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinArea : MonoBehaviour {

    // Use this for initialization
    Player player;
    miniMapBox miniMapBox;

    WinScreen winScreen;
    public void ActiveWinMiniMap()
    {
        miniMapBox.gameObject.SetActive(true);
    }

    private void Awake()
    {
        Time.timeScale = 1;
    }

    private void Start()
    {
        winScreen = FindObjectOfType<WinScreen>();
        miniMapBox = GetComponentInChildren<miniMapBox>();
        miniMapBox.gameObject.SetActive(false);
        winScreen.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            player = other.GetComponent<Player>();
            if (player.winObjectHeld)
            {
                winScreen.gameObject.SetActive(true);
                player.DisableCam();
                Time.timeScale = 0;
            }
        }
    }
}
