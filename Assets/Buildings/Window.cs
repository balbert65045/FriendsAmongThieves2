using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    float UpwardsSpeedforVaultOutside = 0f;
    [SerializeField]
    float UpwardsSpeedforVaultinside= 0f;


    OutideWindowPosition outsideWindowPosition;
    InsideWindowPosition insideWindowPosition;


    public void playerUsingWindow(int location, GameObject player)
    {
        //1 is inside 
        //2 is outside
        if (location == 1)
        {

            Debug.Log("Player at Window Inside");
        }
        else if (location == 2)
        {
          //  player.transform.LookAt(this.transform);
            Debug.Log("Player at Window Outside");
        }
    }


	void Start () {
        outsideWindowPosition = GetComponentInChildren<OutideWindowPosition>();
        //outsideWindowPosition.SetSpeedToVault(UpwardsSpeedforVaultOutside);

        insideWindowPosition = GetComponentInChildren<InsideWindowPosition>();
     //   insideWindowPosition.SetSpeedToVault(UpwardsSpeedforVaultinside);
    }
	



	// Update is called once per frame
	void Update () {
        outsideWindowPosition.SetSpeedToVault(UpwardsSpeedforVaultOutside);
        insideWindowPosition.SetSpeedToVault(UpwardsSpeedforVaultinside);
    }
}
