using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (collision.transform.GetComponent<MyThirdPersonCharacter>())
        {
            collision.transform.GetComponent<MyThirdPersonCharacter>().SetSleep(SleepTime);
        }

        Destroy(gameObject);

    }
}
