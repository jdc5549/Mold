using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drop : MonoBehaviour {
    //float speed = 0.05f;
	public float counter = 0;
	public float wait_frames = 1200;
	float rotate_speed = 20f;
	Rigidbody rigid;



    // Use this for initialization
    void Start () {
		rigid = gameObject.GetComponent<Rigidbody>();
		counter = 0;
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    // Update is called once per frame
    void Update()
    {
		
		//if (rigid.constraints != RigidbodyConstraints.FreezeAll && counter < wait_frames)
		//	rigid.constraints = RigidbodyConstraints.FreezeAll;

		if (counter <= wait_frames) {
			//propeller.transform.Rotate (new Vector3 (0, 0, 1), rotate_speed);
			counter++;
		}
		else{
			rigid.constraints = RigidbodyConstraints.None;
		}


    }
}
