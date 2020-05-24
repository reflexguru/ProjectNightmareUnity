using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float Speed;
    float grava = 0f;

    //sounds

    public AudioSource asrc;
    public AudioClip jumpsound;

    void Start() {
        asrc = GetComponent<AudioSource>();
    }

	void Update() {
        Movement();
    }

    void Movement() {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        // grava == gravitation xd
        if (
            Input.GetKey("space") && 
            grava == 0 && !GetComponent<Rigidbody>().isKinematic && 
            Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 1.15f)
        ) {
            grava = 1.2f;
            asrc.PlayOneShot(jumpsound);
        }

        Vector3 playerMovement = new Vector3(hor, grava, ver) * Speed * Time.deltaTime;
        transform.Translate(playerMovement, Space.Self);

        if (grava > 0f)
            GetComponent<Rigidbody>().isKinematic = true;
            grava -= 1.6f * Time.deltaTime;

        if (grava < 0f) {
            if (GetComponent<Rigidbody>().isKinematic)
                GetComponent<Rigidbody>().isKinematic = false;
            grava = 0f;
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), 1.15f))
            grava = 0f;
    }
}
