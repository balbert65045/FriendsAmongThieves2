using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Cameras;

public class PlayerBuilder : NetworkBehaviour {

    public GameObject CameraRig;
    public GameObject SoundSphere;
    public GameObject MiniMapSystem;
    public GameObject PlayerCanvas;

    public override void OnStartLocalPlayer()
    {
        GameObject MyCAmeraRig = Instantiate(CameraRig);
        MyCAmeraRig.GetComponent<MyFreeLookCam>().SetTarget(this.transform);

        GameObject MySoundSphere = Instantiate(SoundSphere);
        MySoundSphere.GetComponent<SoundSphere>().SetTarget(this.transform);

        GameObject MyMiniMapSystem = Instantiate(MiniMapSystem);
        MyMiniMapSystem.GetComponentInChildren<miniMap>().SetPlayer(this.GetComponent<Player>());

        GameObject MyPlayerCanvas = Instantiate(PlayerCanvas);
        FindObjectOfType<StaminaSlider>().SetPlayer(this.GetComponent<Player>());

        KeyImage[] KeyImages = MyPlayerCanvas.GetComponentsInChildren<KeyImage>();
        foreach(KeyImage key in KeyImages)
        {
            GetComponent<Inventory>().LinkKey(key);
        }
    }
  
}
