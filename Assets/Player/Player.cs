﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.Cameras;
using System;

public class Player : MonoBehaviour {


    public GameObject rock;

    [SerializeField]
    float StaminaDecreaseRate = 2f;
    [SerializeField]
    float StaminaRechargeRate = 2f;
    [SerializeField]
    float RunPercentLimit = 20;
    [SerializeField]
    float BallLaunchForce = 500;
    [SerializeField]
    float BallUpForce = 200;

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

    ThirdPersonUserControl thirdPersonUserControl;
    FreeLookCam freeLookCam;

    ThirdPersonCharacter thirdPersonCharacter;
     
    // Not needed at the moment
    public bool VaultAreaInside = false;


    Vector3 windowStartPosition;
    Transform WindowTransform;

    UsableItemsUI usableItemsUI;
    UsableObject currentItemUsing = UsableObject.Rock;

    public void ObjectGrabbed()
    {
        winObjectHeld = true;
        notifyOnWinItemHelddObservers(winObjectHeld);
    }

    public void DisableCam(){freeLookCam.enabled = false;}

    //public void InVaultArea(Vector3 startPosition, Transform window, float speed)
    //{
    //    windowStartPosition = startPosition;
    //    WindowTransform = window;
    //    thirdPersonCharacter.VaultUpSpeed = speed;
    //}

    public void RelockCursor(){ freeLookCam.LockCursor();}

    public void AddItem(GameObject Item)
    {
        inventory.AddItem(Item);
        int amount = inventory.QuantityCheck(currentItemUsing);
        usableItemsUI.ShowNewQuantity(amount);
    }

    // Use this for initialization
    void Start () {
        freeLookCam = FindObjectOfType<FreeLookCam>();
        m_rigidbody = GetComponent<Rigidbody>();
        thirdPersonUserControl = GetComponent<ThirdPersonUserControl>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
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
        //use object when right mouse button is clicked
        if (Input.GetButtonDown("UseObject")){TryToUseObjectHolding();}
        if (Input.GetButtonDown("ChangeObject")){ChangeObjectHolding();}
        if (Input.GetButtonDown("Action")){ CheckForAction();}
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
            if (Hit.transform.GetComponent<Door>())
            {
                // Test if its a general door
                Hit.transform.GetComponent<Door>().OpenCloseDoor(Door.DoorType.General);
                // Then try all keys 
                foreach ( Key key in inventory.keys)
                {
                    if (Hit.transform.GetComponent<Door>().OpenCloseDoor(key.KeyType))
                    {
                        break;
                    }
                }
            }
            else if (Hit.transform.GetComponent<Chest>())
            {
                Hit.transform.GetComponent<Chest>().OpenChest(this);
                freeLookCam.UnlockCursor();
            }

        }
    }

    /// <summary>
    //private void ChecktoVaultWindow()
    //{
    //    if (VaultAreaInside && PlayerAction)
    //    {
    //        Debug.Log("VaultAction!");
    //        thirdPersonUserControl.MoveDisabled = true;
    //        transform.position = windowStartPosition;
    //        transform.LookAt(WindowTransform);
    //        thirdPersonCharacter.VaultAction();
    //    }

    //    else if (!VaultAreaInside)
    //    {
    //        thirdPersonUserControl.MoveDisabled = false;
    //    }
    //}
    /// </summary>

    // Launches the rock from above head 
    //TODO make this an ability to be able to aim
    private void ThrowRock()
    {
        GameObject rock1 = Instantiate(rock, itemSocket.transform.position, Quaternion.identity);
        rock1.GetComponent<Rigidbody>().AddForce(transform.forward * BallLaunchForce + transform.up * BallUpForce);
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
    private void OnTriggerEnter(Collider other)
    {
        //Caught/ Dead
        if (other.GetComponent<EnemyAttackSphere>())
        {
            loseScreen.gameObject.SetActive(true);
            DisableCam();
            Time.timeScale = 0;
        }
    }



}
