using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SleepDart : NetworkBehaviour {

    [SerializeField]
    float SleepTime = 1f;

	

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<MyThirdPersonCharacter>())
        {

            collision.transform.GetComponent<MyThirdPersonCharacter>().RpcSetSleep(SleepTime);
        }

        Destroy(gameObject);

    }
}
