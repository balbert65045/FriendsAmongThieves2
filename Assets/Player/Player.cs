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



        if (Input.GetMouseButtonDown(1))
        {
            ThrowRock();
        }

        if (Input.GetButtonDown("Action"))
        {
            CheckForDoor();
        }



        // Check if player is vaulting window
        ChecktoVaultWindow();

        //Deplete Stamina when running and recharge anytime else
        UpdateStamina();

        //Adjust the sound made by your speed
        AdjustSound();

    }

    private void CheckForDoor()
    {
        RaycastHit Hit;
        Ray DoorRay = new Ray(transform.position + Vector3.up, transform.forward);
        if (Physics.Raycast(DoorRay, out Hit, 2f))
        {
            Debug.Log(Hit.transform.name);
            if (Hit.transform.GetComponent<Door>())
            {
                foreach( Key key in inventory.keys)
                {
                    if (Hit.transform.GetComponent<Door>().OpenCloseDoor(key.KeyType))
                    {
                        break;
                    }
                }
            }
        }
    }


    private void ChecktoVaultWindow()
    {
        if (VaultAreaInside && PlayerAction)
        {
            Debug.Log("VaultAction!");
            thirdPersonUserControl.MoveDisabled = true;
            transform.position = windowStartPosition;
            transform.LookAt(WindowTransform);
            thirdPersonCharacter.VaultAction();
        }

        else if (!VaultAreaInside)
        {
            thirdPersonUserControl.MoveDisabled = false;
        }
    }

    private void ThrowRock()
    {
        GameObject rock1 = Instantiate(rock, itemSocket.transform.position, Quaternion.identity);
        rock1.GetComponent<Rigidbody>().AddForce(transform.forward * BallLaunchForce + transform.up * BallUpForce);
    }

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
