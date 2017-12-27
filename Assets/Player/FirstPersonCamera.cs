using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class FirstPersonCamera : MonoBehaviour {

    //[SerializeField]
   
    [SerializeField]
    float zOffset = .2f;
    [SerializeField]
    float m_TurnSpeed = 1f;
    [SerializeField]
    float m_TiltMin = 45f;
    [SerializeField]
    float m_TiltMax = 75f;
    // Use this for initialization
    Player player;
    float height;
    float m_LookAngle;
    float m_TiltAngle;


    void Start () {
        player =  FindObjectOfType<Player>();
        height = player.GetComponent<CapsuleCollider>().height;

    }
	
	// Update is called once per frame
	void Update () {
        height = player.GetComponent<CapsuleCollider>().height;
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + height, player.transform.position.z + zOffset);

        var x = CrossPlatformInputManager.GetAxis("Mouse X");
        var y = CrossPlatformInputManager.GetAxis("Mouse Y");

        m_LookAngle += x * m_TurnSpeed;
        m_TiltAngle = Mathf.Clamp(m_TiltAngle, -m_TiltMin, m_TiltMax);
        transform.rotation = Quaternion.Euler(m_TiltAngle, m_LookAngle, 0f);

    }
}
