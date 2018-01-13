using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;
using System;

using UnityEngine.Networking;

public class Player : NetworkBehaviour {


    public GameObject rock;
    public GameObject SleepDart;

    [SerializeField]
    float StaminaDecreaseRate = 2f;
    [SerializeField]
    float StaminaRechargeRate = 2f;
    [SerializeField]
    float RunPercentLimit = 20;

    [SerializeField]
    float BallLaunchForce = 500;
    //[SerializeField]
    //float BallUpForce = 200;
    [SerializeField]
    float DartLaunchForce = 500;
    //[SerializeField]
    //float DartUpForce = 200;

    public float CurrentStamina = 100f;

    
    public delegate void OnWinItemHeld(bool ItemHeld); // declare new delegate type
    public event OnWinItemHeld notifyOnWinItemHelddObservers;

    public bool PlayerAction { get; private set; }
    public bool winObjectHeld { get; private set; }

    LoseScreen loseScreen;
    Rigidbody m_rigidbody;
    SoundSphere soundSphere;
    SphereCollider SoundCollider;
    itemSocket itemSocket;
    Inventory inventory;

    MyThirdPersonUserControl thirdPersonUserControl;
    MyFreeLookCam freeLookCam;
    MyThirdPersonCharacter thirdPersonCharacter;
     
    // Not needed at the moment
    public bool VaultAreaInside = false;

    UsableItemsUI usableItemsUI;
    UsableObject currentItemUsing = UsableObject.Rock;

    Chest ChestActiveWith;


    public void ObjectGrabbed()
    {
        winObjectHeld = true;
        notifyOnWinItemHelddObservers(winObjectHeld);
    }

    public void DisableCam(){freeLookCam.enabled = false;}

    public void RelockCursor(){ freeLookCam.LockCursor();}

    //This is where we take the item
    public void TakeItemFromChest(usableItem Item)
    {
        inventory.AddItem(Item.gameObject);
        int amount = inventory.QuantityCheck(currentItemUsing);
        usableItemsUI.ShowNewQuantity(amount);

        // See what index number the object is
        List<GameObject> items = FindObjectOfType<ItemLookUpTable>().Items;
        int itemIndex = -1;
        for (int i = 0; i < items.Count; i++)
        {
            if (Item.gameObject == items[i])
            {
                itemIndex = i;
            }
        }
       
        CmdRemoveItemFromChest(itemIndex, ChestActiveWith.gameObject);
    }

    [Command]
    void CmdRemoveItemFromChest(int itemIndex, GameObject obj)
    {
        NetworkIdentity chestNetID = obj.GetComponent<NetworkIdentity>();
        chestNetID.AssignClientAuthority(connectionToClient);
        obj.GetComponent<Chest>().TakeItemOut(itemIndex);
        chestNetID.RemoveClientAuthority(connectionToClient);
    }


    [Command]
     void CmdOpenChest(GameObject obj)
    {
        NetworkIdentity objNetID = obj.GetComponent<NetworkIdentity>();
        objNetID.AssignClientAuthority(connectionToClient);
        obj.GetComponent<Chest>().SetChestOpen();
        objNetID.RemoveClientAuthority(connectionToClient);
    }

    public void CloseChest()
    {
        CmdCloseChest(ChestActiveWith.gameObject);
    }

    [Command]
     void CmdCloseChest(GameObject obj)
    {
        NetworkIdentity objNetID = obj.GetComponent<NetworkIdentity>();
        objNetID.AssignClientAuthority(connectionToClient);
        obj.GetComponent<Chest>().SetCloseChest();
        objNetID.RemoveClientAuthority(connectionToClient);
    }


    // Use this for initialization
    void Start () {
        freeLookCam = FindObjectOfType<MyFreeLookCam>();
        m_rigidbody = GetComponent<Rigidbody>();
        thirdPersonUserControl = GetComponent<MyThirdPersonUserControl>();
        thirdPersonCharacter = GetComponent<MyThirdPersonCharacter>();
        soundSphere = FindObjectOfType<SoundSphere>();
        SoundCollider = soundSphere.GetComponent<SphereCollider>();
        itemSocket = GetComponentInChildren<itemSocket>();
        winObjectHeld = false;
        thirdPersonUserControl.notifyOnActionPressedObservers += ActionPressed;
        loseScreen = FindObjectOfType<LoseScreen>();
        inventory = GetComponent<Inventory>();
        usableItemsUI = FindObjectOfType<UsableItemsUI>();

        if (loseScreen) { loseScreen.gameObject.SetActive(false); }

    }

    void ActionPressed(bool actionPressed){PlayerAction = actionPressed;}

    // Update is called once per frame
    void Update ()
    {
        if (!isLocalPlayer) { return; }
            //use object when right mouse button is clicked
            if (Input.GetButtonDown("UseObject")) { TryToUseObjectHolding(); }
            if (Input.GetButtonDown("ChangeObject")) { ChangeObjectHolding(); }
            if (Input.GetButtonDown("Action")) { CheckForAction(); }
            //if (Input.GetButtonDown("Zoom")) { freeLookCam.GetComponent<ProtectCameraFromWallClip>().ZoomIn(); }
            //else if (Input.GetButtonUp("Zoom")) { freeLookCam.GetComponent<ProtectCameraFromWallClip>().ZoomkOut(); }
        

        //Deplete Stamina when running and recharge anytime else
        UpdateStamina();
        //Adjust the sound made by your speed
        AdjustSound();
    }

    // uses the current active object 
    private void TryToUseObjectHolding()
    {
        if (inventory.QuantityCheck(currentItemUsing) > 0)
        {
            switch (currentItemUsing)
            {
                case UsableObject.Rock:
                    ThrowRock();
                    break;
                case UsableObject.SleepDart:
                    ThrowDart();
                    break;
                case UsableObject.SmokeBomb:

                    break;
            }
            inventory.LoseItem(currentItemUsing);
            int amount = inventory.QuantityCheck(currentItemUsing);
            usableItemsUI.ShowNewQuantity(amount);
        }
    }

    // Changes between rock, sleep dart, and smoke bomb using
    private void ChangeObjectHolding()
    {
        switch (currentItemUsing)
        {
            case UsableObject.Rock:
                currentItemUsing = UsableObject.SleepDart;
                break;
            case UsableObject.SleepDart:
                currentItemUsing = UsableObject.SmokeBomb;
                break;
            case UsableObject.SmokeBomb:
                currentItemUsing = UsableObject.Rock;
                break;
        }
        int amount = inventory.QuantityCheck(currentItemUsing);
        usableItemsUI.SwitchObject(currentItemUsing, amount);
    }

    // Looks to Open Door or Open Chest
    private void CheckForAction()
    {
        RaycastHit Hit;
        Ray ActionRay = new Ray(transform.position + Vector3.up * .5f, transform.forward);
        if (Physics.Raycast(ActionRay, out Hit, 2f))
        {
            if (Hit.transform.GetComponentInParent<Door>())
            {
                Door.DoorType DoorType = Hit.transform.GetComponentInParent<Door>().ThisDoorType;

                switch (DoorType)
                {
                    case Door.DoorType.General:
                        CmdOpenCloseDoor(Hit.transform.GetComponentInParent<Door>().gameObject);
                        break;
                    case Door.DoorType.Blue:
                        if (inventory.BlueKeyHeld.activeSelf) { CmdOpenCloseDoor(Hit.transform.GetComponentInParent<Door>().gameObject); }
                        break;
                    case Door.DoorType.Red:
                        if (inventory.RedKeyHeld.activeSelf) { CmdOpenCloseDoor(Hit.transform.GetComponentInParent<Door>().gameObject); }
                        break;
                    case Door.DoorType.Green:
                        if (inventory.GreenKeyHeld.activeSelf) { CmdOpenCloseDoor(Hit.transform.GetComponentInParent<Door>().gameObject); }
                        break;
                    case Door.DoorType.Gold:
                        if (inventory.GoldKeyHeld.activeSelf) { CmdOpenCloseDoor(Hit.transform.GetComponentInParent<Door>().gameObject); }
                        break;
                }
            }
            else if (Hit.transform.GetComponent<Chest>())
            {
                if (!Hit.transform.GetComponent<Chest>().InUse)
                {
                    Hit.transform.GetComponent<Chest>().OpenChest(this);
                    CmdOpenChest(Hit.transform.gameObject);
                    ChestActiveWith = Hit.transform.GetComponent<Chest>();
                    freeLookCam.UnlockCursor();
                }
            }

        }
    }


    [Command]
     void CmdOpenCloseDoor(GameObject obj)
    {
        NetworkIdentity objNetID = obj.GetComponent<NetworkIdentity>();
        objNetID.AssignClientAuthority(connectionToClient);
        obj.GetComponent<Door>().RpcOpenCloseDoor();
        objNetID.RemoveClientAuthority(connectionToClient);
    }


    // Launches the rock from above head 
    //TODO make this an ability to be able to aim
    private void ThrowRock()
    {
        MyFreeLookCam rig = FindObjectOfType<MyFreeLookCam>();
        Ray AimRay = rig.GetComponentInChildren<Camera>().ScreenPointToRay(FindObjectOfType<AimReticle>().transform.position);
        Quaternion rotation = Quaternion.LookRotation(AimRay.direction);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        GameObject rock1 = Instantiate(rock, itemSocket.transform.position, Quaternion.identity);
        rock1.GetComponent<Rigidbody>().AddForce(AimRay.direction * BallLaunchForce);
        CmdSpawnRock(rock1);
    }

    [Command]
    void CmdSpawnRock(GameObject rock)
    {
        NetworkServer.Spawn(rock);
    }


    private void ThrowDart()
    {
        MyFreeLookCam rig = FindObjectOfType<MyFreeLookCam>();
        Ray AimRay = rig.GetComponentInChildren<Camera>().ScreenPointToRay(FindObjectOfType<AimReticle>().transform.position);
        Quaternion rotation = Quaternion.LookRotation(AimRay.direction);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        GameObject MySleepDart = Instantiate(SleepDart, itemSocket.transform.position, transform.rotation);
        MySleepDart.GetComponent<Rigidbody>().AddForce(AimRay.direction * DartLaunchForce);
    }

    // Deplete the stamina when moving and recharge when not moving
    private void UpdateStamina()
    {
         if (thirdPersonUserControl.Running && !thirdPersonUserControl.crouch)
        {
            CurrentStamina -= Time.deltaTime * StaminaDecreaseRate;
            if (CurrentStamina <= 0)
            {
                CurrentStamina = 0;
                thirdPersonUserControl.EnableRun = false;
            }
        }
        else
        {
            if (CurrentStamina < 100) { CurrentStamina += Time.deltaTime * StaminaRechargeRate; }
            if (!thirdPersonUserControl.EnableRun && CurrentStamina > RunPercentLimit) { thirdPersonUserControl.EnableRun = true; }
        }
    }

    // Adjust sound according to speed
    private void AdjustSound()
    {
        if (m_rigidbody.velocity.magnitude != 0)
        {
            SoundCollider.radius = m_rigidbody.velocity.magnitude * 1.2f;
        }
        else
        {
            SoundCollider.radius = 0;
        }
    }

    //TODO take damage and fall when die 
    //private void OnTriggerEnter(Collider other)
    //{
    //    //Caught/ Dead
    //    if (other.GetComponent<EnemyAttackSphere>())
    //    {
    //        loseScreen.gameObject.SetActive(true);
    //        DisableCam();
    //        Time.timeScale = 0;
    //    }
    //}



}
