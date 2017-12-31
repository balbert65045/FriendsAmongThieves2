using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class SleepDart : MonoBehaviour {

    [SerializeField]
    float SleepTime = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<ThirdPersonCharacter>())
        {
            collision.transform.GetComponent<ThirdPersonCharacter>().SetSleep(SleepTime);
        }

        Destroy(gameObject);

    }
}
