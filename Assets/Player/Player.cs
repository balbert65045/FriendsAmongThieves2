using System.Collections;
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
     
    public bool VaultAreaInside = false;
    Vector3 windowStartPosition;
    Transform WindowTransform;

    public void ObjectGrabbed()
    {
        winObjectHeld = true;
        notifyOnWinItemHelddObservers(winObjectHeld);
    }

    public void DisableCam()
    {
        freeLookCam.enabled = false;
    }

    public void InVaultArea(Vector3 startPosition, Transform window, float speed)
    {
        windowStartPosition = startPosition;
        WindowTransform = window;
        thirdPersonCharacter.VaultUpSpeed = speed;


    }

    public void RelockCursor()
    {
        freeLookCam.LockCursor();
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

        if (loseScreen) { loseScreen.gameObject.SetActive(false); }

    }

    void ActionPressed(bool actionPressed)
    {
        PlayerAction = actionPressed;
    }

    // Update is called once per frame
    void Update ()
    {


        //TODO need to only do this if rock is equiped or inventory


        Debug.DrawLine(transform.position + Vector3.up * .5f, transform.position + Vector3.up * .5f + transform.forward * 2);
        if (Input.GetMouseButtonDown(1))
        {
            ThrowRock();
        }

        if (Input.GetButtonDown("Action"))
        {
            CheckForAction();
        }

        //Deplete Stamina when running and recharge anytime else
        UpdateStamina();

        //Adjust the sound made by your speed
        AdjustSound();

    }

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
