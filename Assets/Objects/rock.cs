using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rock : MonoBehaviour {

    [SerializeField]
    GameObject SoundSphere;
    [SerializeField]
    float SoundDistance = 5f;
    [SerializeField]
    float TimeAfterHit = 1f;

    GameObject Sound;
    float TimeSounded;
  

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Sound)
        {
            if(TimeSounded + TimeAfterHit < Time.time)
            {
                Destroy(Sound);
                Destroy(gameObject);
            }
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        //Terrain object layer is 8 
        if (collision.gameObject.layer == 8)
        {
            if (!Sound)
            {
                Sound = Instantiate(SoundSphere, transform.position, Quaternion.identity);
                Sound.GetComponent<SphereCollider>().radius = SoundDistance;
                TimeSounded = Time.time;
            }
        }
    }

}
