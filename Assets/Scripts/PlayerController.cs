using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float Speed;
    float grava = 0f;

	void Update() {
        Movement();
    }

    void Movement() {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        // grava == gravitation xd
        if (Input.GetKey("space") && grava == 0 && !GetComponent<Rigidbody>().isKinematic) {
            grava = 0.6f;
        }

        Vector3 playerMovement = new Vector3(hor, grava, ver) * Speed * Time.deltaTime;
        transform.Translate(playerMovement, Space.Self);

        if (grava > 0f)
            GetComponent<Rigidbody>().isKinematic = true;
            grava -= 0.8f * Time.deltaTime;

        if (grava < 0f) {
            if (GetComponent<Rigidbody>().isKinematic)
                GetComponent<Rigidbody>().isKinematic = false;
            grava = 0f;
        }
    }
}
