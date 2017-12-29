using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutideWindowPosition : MonoBehaviour {

    Window window;
    float SpeedtoVault;
    public void SetSpeedToVault(float speed)
    {
        SpeedtoVault = speed;
    }

    private void Start()
    {
        window = GetComponentInParent<Window>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            // 2 is outside;
            //window.playerUsingWindow(2, other.gameObject);
            Debug.Log("In Window Position");
            other.gameObject.GetComponent<Player>().VaultAreaInside = true;
           // other.gameObject.GetComponent<Player>().InVaultArea(this.transform.position, transform.parent, SpeedtoVault);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            other.gameObject.GetComponent<Player>().VaultAreaInside = false;
        }
    }
}
