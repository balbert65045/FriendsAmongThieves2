using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;


    [RequireComponent(typeof(MyThirdPersonCharacter))]
    public class MyThirdPersonUserControl : NetworkBehaviour
    {
        private MyThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
        public bool crouch { get; private set; }

        public bool Running { get; private set; }

        public delegate void OnActionPressed(bool Action); // declare new delegate type
        public event OnActionPressed notifyOnActionPressedObservers;

        public bool Action { get; private set; }

        public bool MoveDisabled = false;
        public bool EnableRun = true;

        private void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;

            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<MyThirdPersonCharacter>();
        }


        private void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                crouch = !crouch;
            }


            if (notifyOnActionPressedObservers != null) { notifyOnActionPressedObservers(Input.GetKeyDown(KeyCode.Q)); }

            //if (Input.GetKeyDown(KeyCode.Q))
            //{
            //    if (notifyOnActionPressedObservers != null)  {  notifyOnActionPressedObservers(true); }
            //}
            //else if (Input.GetKeyUp(KeyCode.Q))
            //{
            //    if (notifyOnActionPressedObservers != null) { notifyOnActionPressedObservers(false); }
            //}

        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            if (!isLocalPlayer)
            {
                return;
            }
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            //bool crouch = Input.GetKey(KeyCode.C);



            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }


            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift) && EnableRun)
            {
                m_Move *= 8f;
                Running = true;
            }
            else
            {
                m_Move.Normalize();
                m_Move *= .5f;
                Running = false;
            }

            if (MoveDisabled) { m_Move = Vector3.zero; }
            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;
        }
    }